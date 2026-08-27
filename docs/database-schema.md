## Database Schema - Library Management System

This document describes the database design for the Library Management System API.
The system tracks books, authors, categories, physical book copies, members, and loan transactions, and is built with SQL Server using EF Core Code-First migrations.

## Entity Relationship Diagram

```mermaid
erDiagram
      AUTHOR ||--o{ BOOKAUTHOR: writes
      BOOK ||--o{ BOOKAUTHOR: "written by"
      CATEGORY ||--O{ BOOK : categorizes
      BOOK ||--O{ BOOKCOPY : "has copies"
      BOOKCOPY ||--O{ LOAN : "loaned as"
      MEMBER ||--O{ LOAN : borrows

      AUTHOR {
          int Id PK
          string FirstName
          string LastName
      }
      BOOK{
          int Id PK
          string Title
          string ISBN
          int CategoryId FK
      }
      BOOKAUTHOR{
          int BookId PK,FK
          int AuthorId PK,FK
      }
      CATEGORY {
          int Id PK
          string Name
      }

      BOOKCOPY{
          int Id PK
          int BookId FK
          string CopyNumber
          string Condition
          bool IsAvailable
      }
      MEMBER{
          int Id PK
          string FullName
          string Email
          datetime JoinedAt
      }
      LOAN {
          int id PK
          int BookCopyId FK
          int MemberId FK
          datetime BorrowedAt
          datetime DueAt
          datetime? ReturnedAt
      }
```

## Tables

### Author
Stores the book authors.

| Column | Type | Constraints | Notes|
|---|---|---|---|
| Id | int | PK, Identity | |
| FirstName| nvarchar(100) | NOT NULL | |
| LastName | nvarchar(100) | NOT NULL | |

### Category
Book genres.

| Column | Type | Constraints | Notes|
|---|---|---|---|
| Id | int | PK, Identity | |
| Name | nvarchar(50) | NOT NULL, UNIQUE | |


### Book
A title in the catalog (not a physical item).

| Column | Type | Constraints | Notes|
|---|---|---|---|
| Id | int | PK, Identity | |
| Title | nvarchar(200) | NOT NULL | |
| ISBN | nvarchar(20) | UNIQUE | |
| CategoryId | int | FK -> Category.Id | |


### BookAuthor
Join table resolving the many to many between book and author,

| Column | Type | Constraints | Notes|
|---|---|---|---|
| BookId | int | PK, FK->Book.Id | |
| AuthorId | int | PK, FK -> Author.Id | Composite PK with BookId |

### BookCopy
A physical, loanable copy of a book.

| Column | Type | Constraints | Notes|
|---|---|---|---|
| Id | int | PK, Identity | |
| BookId | int | FK -> Book.Id | |
| CopyNumber | nvarchar(20) | NOT NULL | e.g. "COPY-001" |
| Condition | nvarchar(20) | NOT NULL | e.g. New, Good, Worn |
| IsAvailable | bit | NOT NULL, default true | Denormalized flag |


### Member
A library member who can borrow books.

| Column | Type | Constraints | Notes|
|---|---|---|---|
| Id | int | PK, Identity | |
| FullName | nvarchar(150) | NOT NULL | |
| Email | nvarchar(150) | UNIQUE, NOT NULL | |
| JoinedAt | datetime | NOT NULL, default GETDATE() | |

### Loan 
A borrowing transaction linking a member to a bookcopy.

| Column | Type | Constraints | Notes|
|---|---|---|---|
| Id | int | PK, Identity | |
| BookCopyId | int | FK -> BookCopy.Id | |
| MemberId | int | FK -> Member.Id | |
| BorrowedAt | datetime | NOT NULL | |
| DueAt | datetime | NOT NULL | |
| ReturnedAt | datetime | NULL | NULL = currently out|



## Relationships

 - One **Author** can write many **Books**, and one **Book** can have many **Authors** (many-to-many via `BookAuthor` )
 - One **Category** has many **Books**, each **Book** belongs to one **Category**
 - One **Book** has many **BookCopies**
 - One **BookCopy** can be loaned many times over its life, but only once at a time
 - One **Member** can have many **Loans**
 - A **Loan** always references a **BookCopy** and a **Member**

