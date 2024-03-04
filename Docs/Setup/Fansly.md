# SubLink Setup Fansly

[Back To Readme](../../README.md)

## Setup

1. On the first run, the application will create a `Settings/Fansly.json` file.
2. Retrieve your Token, using your browser.
   1. Go to the Fansly website.
   2. Open the devtools (`F12` or `right-click`>`inspect element`).
   3. Open the `Network` tab in the devtools.
   4. Search for `method:GET api`, refresh the page and click on one of the requests ending in `ngsw-bypass=true`.
   5. Look at the `Request Headers` and find the `Authorization:` line, copy the value behind it.  
![Token Step 1](https://raw.githubusercontent.com/yewnyx/SubLink/master/Docs/token-step-1.png?raw=true "Token Step 1")  
![Token Step 2](https://raw.githubusercontent.com/yewnyx/SubLink/master/Docs/token-step-2.png?raw=true "Token Step 2")
3. Add the `Authorization:` value in your browser to the `Settings/Fansly.json` file's `Token` setting.
4. Add your username to the `Settings/Fansly.json` file's `Username` setting.
5. On the second run, the application will automatically connect to Fansly' real-time API and start receiving events.

## Config Template

```json
{
  "Fansly": {
    "Token": "your-fansly-token",
    "Username": "your-username"
  }
}
```
