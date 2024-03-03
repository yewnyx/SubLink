# SubLink DataTypes Fansly Types

[Back To Readme](../../../README.md)  
[Back To Fansly DataTypes Index](Index.md)

## ChatMessageEvent

- `string` Id          - Required - The message ID
- `string` ChatRoomId  - Required - The chatroom ID
- `string` SenderId    - Required - The sender's user ID
- `string` Username    - Required - The sender's username
- `string` Displayname - Required - The sender's displayname
- `string` Content     - Required - The message content
- `long`   CreatedAt   - Required - The message creation timestamp

## TipEvent

- `string` Id          - Required - The message ID
- `string` ChatRoomId  - Required - The chatroom ID
- `string` SenderId    - Required - The sender's user ID
- `string` Username    - Required - The sender's username
- `string` Displayname - Required - The sender's displayname
- `string` Content     - Optional - The message content
- `float`  Amount      - Required - The tip amount in USD
- `int`    CentAmount  - Required - The tip amount in USD cents
- `long`   CreatedAt   - Required - The message creation timestamp

## GoalUpdatedEvent

- `string` Id            - Required - The message ID
- `string` ChatRoomId    - Required - The chatroom ID
- `string` AccountId     - Required - The streamer's account ID
- `uint`   Type          - Required - The goal type
- `string` Label         - Required - The goal title
- `string` Description   - Required - The goal description
- `uint`   Status        - Required - The goal status
- `int`    CurrentAmount - Required - The current amount towards the goal in USD cents
- `int`    GoalAmount    - Required - The goal amount in USD cents
- `uint`   Version       - Required - The revision number of the goal
