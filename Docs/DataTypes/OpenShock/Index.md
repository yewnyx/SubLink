# SubLink DataTypes OpenShock Types

[Back To Readme](../../../README.md)

- [ShockerModelType](#ShockerModelType)
- [ResponseHubWithShockers](#ResponseHubWithShockers)
- [ShockerResponse](#ShockerResponse)

## ShockerModelType

- CaiXianlin     - 0   -
- PetTrainer     - 1   - Misspelled, should be "petrainer"
- Petrainer998DR - 2   -
- Unknown        - 255 - Whenever we fail to parse the model type

## ResponseHubWithShockers

- `Guid`              Id        - Required - Hub ID
- `string`            Name      - Required - Hub name
- `DateTime`          CreatedOn - Required - Hub creation timestamp
- `ShockerResponse[]` Shockers  - Required - Array with owned shockers

## ShockerResponse

- `Guid`             Id        - Required - Shocker ID
- `ushort`           RfId      - Required - Shocker RfID
- `ShockerModelType` Model     - Required - Shocker model
- `string`           Name      - Required - Shocker name
- `bool`             IsPaused  - Required - Shocker paused state
- `DateTime`         CreatedOn - Required - Shocker creation timestamp
