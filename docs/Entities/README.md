# Documentation: Expenses Control Domain Classes

This document provides an overview of the main classes in the Expenses Control domain, including their properties, methods, and usage.

---

## **Class: BaseEntity<TKey>**

### **Description**
Represents the base implementation of an entity with audit fields.

### **Properties**

| Property          | Type        | Description                                               |
|-------------------|-------------|-----------------------------------------------------------|
| `Id`              | `TKey`     | Unique identifier of the entity.                         |
| `CreationDate`    | `DateTime`  | Date and time when the entity was created.               |
| `CreatedByUser`   | `string`    | Username of the user who created the entity.             |
| `LastUpdateDate`  | `DateTime`  | Date and time when the entity was last updated.          |
| `UpdatedByUser`   | `string`    | Username of the user who last updated the entity.        |

### **Methods**

#### `void MarkAsCreated()`
Marks the entity as created, initializing creation and update timestamps.

#### `BaseEntity<TKey> SetCurrentUser(string user)`
Sets the user responsible for the creation and/or update of the entity.

#### `void MarkAsUpdated()`
Marks the entity as updated, updating the timestamp.
