## POS Project DocumentManager
### Group members:
- Tristan Losada Benini, LOS20421@spengergasse.at
- Ruben Osmanovic, OSM21985@spengergasse.at

### Description:
- A Document(.txt/.md files) manager with User sharing and Guest Users that can view the files without being able to update them
- The Document class represents a document. It has methods for updating the document's properties and moving it to a different folder
- The Tag class represents a tag that can be assigned to a document
- The User has the ability to share documents with other users and revoke access to documents
- The GuestUser does not have the ability to share documents with other users or revoke access to documents but can still view them
- The Folder contains documents
- The DocumentManager manages the document system. It has methods for creating, deleting, and searching for documents, as well as granting and revoking access to documents.

### Domain Model (UML):


![](DomainModel.png)
