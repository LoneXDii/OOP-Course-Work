# **Pavlovich Vladislav 253505**

## Project description
My project is a simple client-server desktop messenger. This messenger will include features such as user authorization and the ability to create chats and groups with other users. The project will be written in C#. The messenger will have a simple UI. For data storage, I’ll use a remote database, which will be connected to the desktop application through the server. The server will also be implemented in C# and will be a separate application from the client. It will be used for sending data to the database and retrieving data from it. The server will send information from the client not only to the database, but also to other users to whom this information is relevant. For example, if you send a message to the chat, it will not only be saved in the database, but also sent to other members of this chat in real time.

## Class diagram
  **Client class diagram**
  
  ![image](https://github.com/LoneXDii/OOP-Course-Work/assets/151780058/5832b685-ac8d-4f84-8c70-d6a94fa33ee1)

  Presentation layer
  
  ![image](https://github.com/LoneXDii/OOP-Course-Work/assets/151780058/494d5afe-578b-4cc0-936b-631f53c30742)

  Logick layer
  
  ![image](https://github.com/LoneXDii/OOP-Course-Work/assets/151780058/1835e2c1-0442-4d5b-b9d0-c8ceda20f2c2)

  Data layer
  
  ![image](https://github.com/LoneXDii/OOP-Course-Work/assets/151780058/c02cb0ed-3145-4680-97b3-86f5cb8f0a3a)

  **Server class diagram**

  ![image](https://github.com/LoneXDii/OOP-Course-Work/assets/151780058/cf687042-22f5-4a94-a42e-2fb04ea8c9ae)

 
## List of functions
1. **User registration and authorization**: This is the process where users create an account by providing necessary information (login and password). After registration, they can log into the system using their credentials. This process ensures that each user has a unique identity in the system.
2. **Creation of chats and groups for communication with other users**: Users can create chats or group chats. Chats are usually between two users, while group chats can include multiple users. This feature allows users to communicate and collaborate with each other.
3. **Ability to send, edit, and delete messages**: Users can send messages to other users in a chat or group. If they make a mistake or want to change something, they have the option to edit their messages. They can also delete their messages if they no longer want them to be visible to others.
4. **Data storage in a remote database**: All the data related to the application, like user information, chat history, group details, etc., are stored in a remote database. This allows the data to be persistent and accessible from different devices.
5. **Implementation of client-server architecture**: The application follows a client-server architecture. The client (desktop application) sends requests to the server, and the server processes these requests and sends back responses. Server sends new data to database and other users or get information from the database. This architecture allows for efficient data management and ensures that the application can handle multiple users at the same time.
6. **Option to change your nickname and password**: Users have the option to change their nickname and password. Nickname is a name, that other users see, login is unchangable.

## Data model
![image](https://github.com/LoneXDii/OOP-Course-Work/assets/151780058/a9decec5-0a1a-4d12-a9bd-0756e21195e1)