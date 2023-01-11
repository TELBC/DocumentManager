## POS Project DocumentManager
### Group members:
- Tristan Losada Benini, LOS20421@spengergasse.at
- Ruben Osmanovic, OSM21985@spengergasse.at

### Description:
- A Document(.txt/.md files) manager with User sharing and Guest Users that can view the files without being able to update them
- The Document class represents a document and has one or more tags
- The Tag class represents a tag that has a category and can be assigned to one or more documents
- The User has the ability to share documents with other users(friends)
- The GuestUser does not have the ability to share documents with other users, but can still view them
- The Folder contains documents
- The DocumentManager has a user "owner" that can influence the folders(and therefor documents) and other users(friends) interaction with those documents

### Domain Model (UML):


![](DomainModel.png)
