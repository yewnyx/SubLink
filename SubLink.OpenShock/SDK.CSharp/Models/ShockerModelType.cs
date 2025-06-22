namespace OpenShock.SDK.CSharp.Models;

public enum ShockerModelType : byte
{
    CaiXianlin = 0,
    PetTrainer = 1, // Misspelled, should be "petrainer",
    Petrainer998DR = 2,
    
    /// <summary>
    /// Whenever we fail to parse the model type
    /// </summary>
    Unknown = byte.MaxValue, 
}