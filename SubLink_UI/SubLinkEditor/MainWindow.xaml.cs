using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml;
using FlowGraph;
using FlowGraph.Logger;
using FlowGraph.Process;
using tech.sublink.SubLinkEditor.FlowGraphs;
using tech.sublink.SubLinkEditor.UI;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;

namespace tech.sublink.SubLinkEditor;

public partial class MainWindow : Window {
    private readonly string _userSettingsFile = "userSettings.xml";
    private readonly string _dockSettingsFile = "dockSettings.xml";

    private MruManager _mruManager;
    private const string RegistryPath = "Software\\SubLink\\Editor";

    private string _fileOpened = "";

    private double _lastLeft, _lastTop, _lastWidth, _lastHeight;

    internal static MainWindow Instance { get; private set; }

    internal FlowGraphManagerControl FlowGraphManagerControl => flowGraphManagerControl;

    internal DetailsControl DetailsControl => detailsControl;

    public MainWindow() {
        InitializeComponent();

        Instance = this;

        //LogManager.Instance.NbErrorChanged += new EventHandler(OnNbErrorChanged);
        Version ver = Assembly.GetExecutingAssembly().GetName().Version;
        statusLabelVersion.Content = "v" + ver;
        SetTitle();

        LogManager.Instance.WriteLine(LogVerbosity.Info, "SubLinkEditor - v{0} started", ver);
        VariableTypeInspector.SetDefaultValues();
        NamedVarEditTemplateManager.Initialize();

        Loaded += OnLoaded;
        Closed += OnClosed;
    }

    void OnLoaded(object sender, RoutedEventArgs e) {
        try {
            _mruManager = new MruManager();
            _mruManager.Initialize(
                this,						// owner form
                menuItemRecentFiles,        // Recent Files menu item
                menuItemFile,		        // parent
                RegistryPath);			// Registry path to keep MRU list

            _mruManager.MruOpenEvent += delegate (object s, MruFileOpenEventArgs openEvent) {
                SaveChangesOnDemand();
                LoadFile(openEvent.FileName);
            };

            LoadSettings();

            if (string.IsNullOrWhiteSpace(_mruManager.GetFirstFileName) == false)
                LoadFile(_mruManager.GetFirstFileName);

            _lastLeft = Left;
            _lastTop = Top;
            _lastWidth = Width;
            _lastHeight = Height;

            ProcessLauncher.Instance.StartLoop();
        } catch (System.Exception ex) {
            LogManager.Instance.WriteException(ex);
        }
    }

    private void OnClosed(object sender, EventArgs e) {
        ProcessLauncher.Instance.StopLoop();
        LogManager.Instance.WriteLine(LogVerbosity.Info, "Closed by user");

        try {
            SaveSettings();
        } catch (System.Exception ex) {
            LogManager.Instance.WriteException(ex);
        }
    }

    private void Resume_Executed(object sender, ExecutedRoutedEventArgs e) =>
        ProcessLauncher.Instance.Resume();

    private void NextStep_Executed(object sender, ExecutedRoutedEventArgs e) =>
        ProcessLauncher.Instance.NextStep();

    private void Pause_Executed(object sender, ExecutedRoutedEventArgs e) =>
        ProcessLauncher.Instance.Pause();

    private void Stop_Executed(object sender, ExecutedRoutedEventArgs e) =>
        ProcessLauncher.Instance.Stop();

    private void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) =>
        NewProject();

    private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) =>
        OpenProject();

    private void SaveCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) =>
        SaveProject();

    private void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) =>
        SaveAsProject();

    private void ExitCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e) =>
        Exit();

    private void menuItemWindows_SubmenuOpened(object sender, RoutedEventArgs e) {
        menuItemWindows.Items.Clear();
        SortedList<string, MenuItem> list = new();

        foreach (var content in dockingManager1.Layout.Descendents().OfType<LayoutContent>()) {
            MenuItem item = new() {
                Header = content.Title,
                IsChecked = content is LayoutDocument document
                    ? document.IsVisible
                    : content is LayoutAnchorable anchorable && anchorable.IsVisible
            };
            item.Click += MenuItemLayout_Click;
            item.Tag = content;
            list.Add(content.Title, item);
        }

        foreach (var menu in list) {
            menuItemWindows.Items.Add(menu.Value);
        }
    }

    private void MenuItemLayout_Click(object sender, RoutedEventArgs e) {
        MenuItem item = sender as MenuItem;
        LayoutAnchorable l = item.Tag as LayoutAnchorable;
        l.IsVisible = !l.IsVisible;
        item.IsChecked = l.IsVisible;
    }

    private void MenuItemWorkspaceSave_Click(object sender, RoutedEventArgs e) =>
        SaveSettings();

    private void MenuIte_OpenDocumentationClick(object sender, EventArgs e) { }

    private void MenuIte_HelpClick(object sender, RoutedEventArgs e) {
        MessageBox msgBox = new();
        windowContainer.Children.Add(msgBox);

        var img = new Image { Source = new BitmapImage(new Uri(
            "pack://application:,,,/SubLinkEditor;component/Resources/SubLink.ico"
        ))};
        msgBox.ImageSource = img.Source;
        msgBox.ShowMessageBox(
            $"SubLink Editor version {Assembly.GetExecutingAssembly().GetName().Version}{Environment.NewLine}" +
            $"by{Environment.NewLine}" +
            $" - Yewnyx{Environment.NewLine}" +
            $" - CatGirlEddie{Environment.NewLine}" +
             " - LauraRozier",
            "Information",
            MessageBoxButton.OK);
    }

    private void Clear() {
        ProjectManager.Clear();

        var list = dockingManager1.Layout.Descendents().OfType<LayoutDocument>().ToList();

        foreach (LayoutDocument ld in list) {
            ld.Close();
        }
    }

    private void NewProject() {
        SaveChangesOnDemand();

        LogManager.Instance.WriteLine(LogVerbosity.Info, "New project");
        LogManager.Instance.NbErrors = 0;

        _fileOpened = "";

        Clear();
        SetTitle();
    }

    private void OpenProject() {
        SaveChangesOnDemand();

        OpenFileDialog form = new() {
            Filter = "Xml files (*.xml)|*.xml",
            Multiselect = false
        };

        if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            LoadFile(form.FileName);

        form.Dispose();
    }

    private void SaveProject() {
        if (string.IsNullOrWhiteSpace(_fileOpened))
            SaveAsProject();
        else
            SaveFile(_fileOpened);
    }

    private void SaveAsProject() {
        SaveFileDialog form = new() {
            Filter = "Xml files (*.xml)|*.xml"
        };

        if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            SaveFile(form.FileName);

        form.Dispose();
    }

    private void Exit() =>
        Close();

    private void workSpaceSaveToolStripMenuIte_Click(object sender, EventArgs e) {
        SaveSettings();
        LogManager.Instance.WriteLine(LogVerbosity.Debug, "Workspace saved");
    }

    private void LoadFile(string fileName, bool addToMru = true) {
        if (File.Exists(fileName)) {
            Clear();

            if (ProjectManager.OpenFile(fileName)) {
                _fileOpened = fileName;
                SetTitle();

                if (addToMru)
                    _mruManager.Add(fileName);
            } else {
                System.Windows.MessageBox.Show(this, $"Can't load the file '{fileName}'. Please check the log.",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _mruManager.Remove(fileName);
            }
        } else {
            System.Windows.MessageBox.Show(this, $"The file '{fileName}' doesn't exist.",
                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            _mruManager.Remove(fileName);
        }
    }

    private void SaveFile(string fileName) {
        if (ProjectManager.SaveFile(fileName)) {
            _mruManager.Add(fileName);
            _fileOpened = fileName;
        }
    }

    private void SaveChangesOnDemand() {
        if ((GraphDataManager.Instance.IsChanges() || FlowGraphManager.IsChanges())
            && System.Windows.MessageBox.Show(
                this, "Save changes ?", "Save ?",  MessageBoxButton.YesNo, MessageBoxImage.Question
            ) == MessageBoxResult.Yes)
            SaveProject();
    }

    private void LoadSettings() {
        double l = Left, t = Top, w = Width, h = Height;
        WindowState winState = WindowState;

        if (File.Exists(_userSettingsFile)) {
            XmlDocument xmlDoc = new();
            xmlDoc.Load(_userSettingsFile);

            XmlNode winNode = xmlDoc.SelectSingleNode("SubLinkEditor/Window");

            int version = int.Parse(winNode.Attributes["version"].Value);
            l = int.Parse(winNode.Attributes["left"].Value);
            t = int.Parse(winNode.Attributes["top"].Value);
            w = int.Parse(winNode.Attributes["width"].Value);
            h = int.Parse(winNode.Attributes["height"].Value);
            winState = (WindowState)Enum.Parse(typeof(WindowState), winNode.Attributes["windowState"].Value);

            XmlNode rootNode = xmlDoc.SelectSingleNode("SubLinkEditor");
        }

        if (File.Exists(_dockSettingsFile)) {
            try {
                var serializer = new XmlLayoutSerializer(dockingManager1);
                serializer.LayoutSerializationCallback += (s, args) => {
                    if (args.Model.Title == "SubLink Graph List")
                        args.Content = flowGraphListContainer;
                    else if (args.Model.Title == "Details")
                        args.Content = detailsGrid;
                    else if (args.Model.Title == "Action Graph")
                        args.Content = containerFlowGraph;
                    else if (args.Model.Title == "Log")
                        args.Content = gridLog;

                    if (args.Content is LayoutAnchorable { CanHide: true, IsHidden: true } layout)
                        layout.Hide();
                };

                using var stream = new StreamReader(_dockSettingsFile);
                serializer.Deserialize(stream);
            } catch (System.Exception ex3) {
                LogManager.Instance.WriteException(ex3);
            }
        }

        Left = l;
        Top = t;
        Width = w;
        Height = h;
        WindowState = winState;
    }

    private void SaveSettings() {
        var serializer = new XmlLayoutSerializer(dockingManager1);
        using (var stream = new StreamWriter(_dockSettingsFile)) {
            serializer.Serialize(stream);
        }

        XmlDocument xmlDoc = new();
        XmlNode rootNode = xmlDoc.AddRootNode("SubLinkEditor");
        rootNode.AddAttribute("version", "1");
        XmlNode winNode = xmlDoc.CreateElement("Window");
        rootNode.AppendChild(winNode);
        winNode.AddAttribute("version", "1");

        if (WindowState == WindowState.Minimized) {
            winNode.AddAttribute("windowState", Enum.GetName(typeof(WindowState), WindowState.Normal));
            winNode.AddAttribute("left", _lastLeft.ToString());
            winNode.AddAttribute("top", _lastTop.ToString());
            winNode.AddAttribute("width", _lastWidth.ToString());
            winNode.AddAttribute("height", _lastHeight.ToString());
        } else {
            winNode.AddAttribute("windowState", Enum.GetName(typeof(WindowState), WindowState));
            winNode.AddAttribute("left", Left.ToString());
            winNode.AddAttribute("top", Top.ToString());
            winNode.AddAttribute("width", Width.ToString());
            winNode.AddAttribute("height", Height.ToString());
        }

        xmlDoc.Save(_userSettingsFile);
    }

    private void SetTitle()
    {
        Title =
#if DEBUG
            "SubLink Editor - DEBUG";
#else
            "SubLink Editor";
#endif

        if (!string.IsNullOrWhiteSpace(_fileOpened))
            Title += $" - {_fileOpened}";
    }
}
