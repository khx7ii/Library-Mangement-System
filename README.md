# Library-Mangement-System

## Overview

The **Library Management System** is an online platform that enables users to browse and borrow books, manage their accounts, and leave comments on books. It keeps track of book availability, user checkouts, and enforces penalties for late returns, ensuring efficient management of library resources and user accountability.

---

## Models

### 1. **Book**

The `Book` model represents the library's book collection.

- **Attributes:**
  - `BookId` (Int): The unique identifier for the book.
  - `Name` (String): The title of the book.
  - `Author` (String): The author of the book.
  - `Quantity` (Int): Number of copies available.
  - `CategoryId` (Int): Foreign key linking the book to a specific category.
  - `Description` (String): Detailed description of the book.

- **Relationships:**
  - Belongs to one `Category`.
  - Can have multiple `Comments`.
  - Can have multiple `Checkouts`.

---

### 2. **Category**

The `Category` model categorizes books.

- **Attributes:**
  - `CategoryId` (Int): Unique identifier for the category.
  - `Name` (String): Category name (e.g., Fiction, Science).

- **Relationships:**
  - Can have multiple `Books`.

---

### 3. **User**

The `User` model represents individuals who can interact with the library system.

- **Attributes:**
  - `UserId` (Int): Unique identifier for the user.
  - `Name` (String): User's name.
  - `Email` (String): User's email address.
  - `Phone` (String): User's phone number.
  - `Password` (String): Account password.

- **Relationships:**
  - Can have multiple `Checkouts`.
  - Can leave multiple `Comments`.
  - Can have multiple `Roles` (via `UserRole`).

---

### 4. **Role**

The `Role` model defines various roles that users can have within the system.

- **Attributes:**
  - `RoleId` (Int): Unique identifier for the role.
  - `RoleName` (String): Name of the role (e.g., User, Admin).

- **Relationships:**
  - Can be assigned to multiple `Users` (via `UserRole`).

---

### 6. **Checkout**

The `Checkout` model tracks book borrowings by users. This includes managing return deadlines and late penalties.

- **Attributes:**
  - `CheckoutId` (Int): Unique identifier for the checkout record.
  - `UserId` (Int): Foreign key referencing the user checking out the book.
  - `BookId` (Int): Foreign key referencing the book being checked out.
  - `CheckoutDate` (DateTime): Date when the book was checked out.
  - `DueDate` (DateTime): Date by which the book should be returned.
  - `ReturnDate` (DateTime): Actual return date of the book (if applicable).
  - `ActiveFlag` (Bool): Flag indicating whether the checkout is active.
  - `Penalty` (Decimal): Fee charged if the book is returned late.

- **Relationships:**
  - A `Checkout` references one `User`.
  - A `Checkout` references one `Book`.

---

### 7. **Comment**

The `Comment` model allows users to leave feedback on books.

- **Attributes:**
  - `CommentId` (Int): Unique identifier for the comment.
  - `UserId` (Int): Foreign key referencing the user leaving the comment.
  - `BookId` (Int): Foreign key referencing the book.
  - `Description` (String): Text content of the comment.

- **Relationships:**
  - A `Comment` references one `User`.
  - A `Comment` references one `Book`.

---

### Entity Relationships Summary

1. **Book - Category**: A `Category` can have multiple `Books`, but each `Book` belongs to one `Category`.

2. **User - Checkout**: A `User` can have multiple `Checkouts`, but each `Checkout` is linked to one `User`.

3. **Book - Checkout**: A `Book` can be involved in multiple `Checkouts`, but each `Checkout` is for one `Book`.

4. **User - Comment**: A `User` can leave multiple `Comments`, but each `Comment` belongs to one `User`.

5. **Book - Comment**: A `Book` can have multiple `Comments`, but each `Comment` is for one `Book`.

6. **User - Book**: A `User` can borrow multiple `Books` through checkouts, and a `Book` can be borrowed and returned by multiple `Users` over time.

---


## Data Flow Diagram (DFD)

### Level 0: Context Diagram
- The system consists of three main actors:
  1. **User**: Interacts with the system to browse, checkout books, and leave comments.
  2. **Admin**: Manages the book catalog, users, and checkout penalties.
  3. **System**: Handles requests for book management, user authentication, checkouts, and comments.

### Level 1: Detailed Data Flow
1. **User Registration/Login**: 
   - **Data Flow**: User → [Input credentials] → System → [Validate credentials] → Database → [User information returned]
2. **Book Browsing**:
   - **Data Flow**: User → [Request book catalog] → System → [Fetch books] → Database → [Books returned] → User
3. **Checkout Process**:
   - **Data Flow**: User → [Request to checkout book] → System → [Validate availability] → Database → [Update book quantity, create checkout record] → User
4. **Return and Penalty Calculation**:
   - **Data Flow**: User → [Return book] → System → [Check return date] → Database → [Calculate penalty, update checkout record] → User

---

## Constraints

1. **Data Integrity**  
   - Users cannot check out a book if the quantity is 0.
   - Users cannot have multiple active checkouts for the same book.

2. **Validation**  
   - Emails must follow proper format (user@example.com).
   - Phone numbers must be in a valid format.

3. **Penalty Calculation**  
   - The penalty is calculated as a flat fee per day of lateness, to be configurable by the admin ($1 per day).

---

## Assumptions

- Users are responsible for returning books on time.
- The system does not handle physical book deliveries, assuming users check out and return books in person.

---

## Risks

1. **Data Breach**:  
   - **Mitigation**: Use encryption for sensitive data, including SSL for data transmission.
2. **System Downtime**:  
   - **Mitigation**: Implement automated backups and redundancy in the server architecture.

---

## Testing

1. **Unit Testing**:  
   - Each individual component (e.g., book checkout logic) will be tested to ensure it functions correctly in isolation.

2. **Integration Testing**:  
   - Test interactions between system components, such as book checkouts and penalty calculations.

3. **User Acceptance Testing (UAT)**:  
   - Conduct UAT to verify that the system meets user needs in real-world scenarios, including user registration, browsing, checkouts, and returns.

---

## System Requirements

### Functional Requirements

1. **User Registration and Authentication**  
   - Users should be able to register with their email, name, phone number, and password.
   - Users should be able to log in using their email and password.

2. **Book Browsing**  
   - Users should be able to browse the library catalog.
   - Users should be able to filter books by category, author, and availability.

3. **Book Checkout**  
   - Registered users should be able to check out available books.
   - The system must record the checkout date, due date, and track the return date.
   - Users can only check out books if they have an active account and there are available copies of the book.

4. **Book Return**  
   - Users should be able to mark a book as returned once they bring it back.
   - The system should update the book’s availability and remove the checkout flag.

5. **Penalty Calculation for Late Returns**  
   - If a book is returned after the due date, a penalty should be applied to the user’s account.
   - The penalty should be based on the number of days the book is overdue.

6. **Commenting System**  
   - Registered users should be able to leave comments on books they have read.
   - The comments should include feedback and ratings (optional).

7. **Role Management**  
   - Admins should be able to assign and remove roles from users.
   - The system should allow users to have multiple roles.

8. **Category Management**  
  

 - Admins should be able to add, edit, or remove book categories.

### Non-Functional Requirements

1. **Performance**  
   - The system should respond to user requests within 2 seconds under normal load.

2. **Security**  
   - User passwords must be securely hashed and stored.
   - Sensitive data should be encrypted during transmission.

3. **Usability**  
   - The interface should be user-friendly and accessible.

4. **Scalability**  
   - The system should support an increasing number of users and books without degradation in performance.

---

## Entity Relationship Diagram (ERD)


![Finance Database ERD](lastone.png)






----

## Class Diagram 

![Finance Database ERD](ll.jpg)



---

## These webpages is designated for administrative access


![Finance Database ERD](1.jpg)



![Finance Database ERD](2.jpg)



![Finance Database ERD](3.jpg)



![Finance Database ERD](4.jpg)



![Finance Database ERD](5.jpg)



![Finance Database ERD](6.jpg)



![Finance Database ERD](7.jpg)



![Finance Database ERD](8.jpg)



![Finance Database ERD](9.jpg)



![Finance Database ERD](10.jpg)



![Finance Database ERD](11.jpg)



![Finance Database ERD](12.jpg)



![Finance Database ERD](13.jpg)



![Finance Database ERD](14.jpg)



![Finance Database ERD](15.jpg)



---

## These webpages is intended for users


![Finance Database ERD](16.jpg)



![Finance Database ERD](17.jpg)




![Finance Database ERD](18.jpg)



![Finance Database ERD](19.jpg)


![Finance Database ERD](20.jpg)



![Finance Database ERD](21.jpg)



![Finance Database ERD](22.jpg)



![Finance Database ERD](23.jpg)



![Finance Database ERD](24.jpg)


![Finance Database ERD](25.jpg)


![Finance Database ERD](26.jpg)


![Finance Database ERD](27.jpg)


![Finance Database ERD](28.jpg)



![Finance Database ERD](29.jpg)



----

## Conclusion

The Library Management System aims to provide a seamless experience for users to interact with library resources. By incorporating roles and user management effectively, it ensures security, usability, and efficient management of library operations. The outlined architecture, models, and requirements set a solid foundation for the development and deployment of the system.

---
