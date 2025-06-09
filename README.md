# Geek Verse E-commerce

**Geek Verse** was born from the idea of bringing together all the classics of geek culture in one place. The goal is to combine my passion for technology with the curiosity and richness of the geek universe, delivering a complete shopping experience for fans of movies, games, books, and streaming.

This project is a **Fullstack Web Development application in C#** designed to simulate a real e-commerce platform, with robust features for both **customers** and **administrators**. Geek Verse is more than just a technical project: it's the fusion of software development and pop culture. It’s a store built by a fan, for fans — focused on scalability, solid architecture, and user experience.


## Project Goal 
### Create a platform that allows:

- A complete end-user experience, from account creation to checkout.

- Full internal management through an admin panel, including control over products, categories, and variations.

### Customer Features

- Create and edit user profile

- Add or remove products from the cart

- View purchase history

- Change password and personal information

- Complete purchases using:

    - PayPal

    - Credit Card

    - Direct Bank Transfer


### Admin Features
Admins are registered directly in the database and granted elevated permissions.
In addition to all customer functionalities, the admin has access to:

- Dashboard with the complete list of available products

- Management of main categories (Movies, Video Games, Books, Streaming)

- Management of subcategories (e.g., E-book, Audiobook, VHS, Blu-ray, PC, PlayStation)

- Add, update, or delete products and categories

- Update product catalog daily


---

# Home Page

![image](https://github.com/user-attachments/assets/c6123e97-dc19-4726-bf47-61f31eaacd45)



---

## Technologies Used

- **.NET Core**: Backend framework for building robust and scalable web applications.
- **Blazor**: Frontend framework for building interactive web UIs using C# instead of JavaScript.
- **AspNetCore Components**: Reusable UI components for building modern web applications.
- **Entity Framework Core**: For database management and object-relational mapping (ORM).
- **Stripe**: API for processing online payments securely.
- **Swashbuckle**: For generating Swagger documentation for the API, making it easier to test and understand endpoints

---

## Getting Started

Follow these steps to set up the project locally:

**Prerequisites:**
.NET SDK (version 6.0 or higher)
Visual Studio or Visual Studio Code
SQL Server or another database supported by Entity Framework Core

---

### Clone the Repository

Configure the Database
1. Open the project in your preferred IDE.
2. Run the following command to apply migrations and create the database: dotnet ef database update

---


### Run the application

1. Navigate to the project directory: cd blazor-ecommerce-v2
2. Start the application: dotnet run
3. Open your browser and navigate to https://localhost:5001 (or the port specified in the console)

### Features

- Product Catalog: Browse and search for products.
- Shopping Cart: Add and manage items in your cart.
- Checkout: Secure payment processing via Stripe, PayPal, or bank transfer.
- User Authentication: Register and log in to manage your orders.
- Admin Panel: Manage products, orders, and users (for administrators).

### Contributing
Contributions are welcome! If you'd like to contribute, please follow these steps:

1. Fork the repository.
2. Create a new branch for your feature or bugfix.
3. Commit your changes and push the branch.
4. Submit a pull request with a detailed description of your changes.
    
### Contact
**Email: lucas_frota@hotmail.com**

[LinkedIn](https://www.linkedin.com/in/lucas-dias-frota-9020b2126/)



```
