# Guest & Room Booking System (Master-Detail)

This is a specialized **ASP.NET MVC** project demonstrating a **Master-Detail** architecture with a Many-to-Many relationship handled via a junction table. It allows a single Guest (Master) to book multiple Rooms (Details) in a single transaction (BookingEntries).

## 🛠️ Tech Stack
- **Framework:** ASP.NET MVC 5
- **ORM:** Entity Framework (Database First Approach)
- **Database:** Microsoft SQL Server
- **Frontend:** Bootstrap, jQuery, & Razor View

## 📊 Relationship Schema
- **Guest (Master):** Stores primary guest information.
- **Room (Details):** Stores available room types and details.
- **BookingEntries (Junction):** Connects Guests and Rooms, enabling "Master-can-book-multiple-rooms" logic.

## 🚀 Getting Started

Follow these steps to set up the project on your local machine:

### 1. Database Setup
The project includes a full database schema script.
1. Open **SQL Server Management Studio (SSMS)**.
2. Open and execute the file named **`database.sql`** located in the project's root folder.

### 2. Download these Packages from Nuget Package manegar
<ul>
  <li>EntityFramework</li>
  <li>Bootstrap 5.2.3</li>
  <li>FontAwosome 4.7.0</li>
  <li>Nwetonsoft json</li>
  <li>Ajax unobstrosive</li>
  <li>pagedList Mvc</li>
</ul>
### 3. Project Opening & Configuration
1. Open the project in **Visual Studio**.
2. Locate the `Web.config` file.
3. Update the `connectionStrings` section to point to your local SQL Server instance:
 
