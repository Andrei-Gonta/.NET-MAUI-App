# __CarMeetApp__

## 📌 Overview

The **CarMeetApp** is a cross-platform mobile/desktop application built with **.NET MAUI** for managing car meet events. The app allows users to discover upcoming automotive events, view event details, register with their car information, upload car photos, and manage their personal profile.

The application also includes an **admin area**, where administrators can create, edit, delete, and review car meet events and their participants.

## 🚀 Features

- 🚗 **Car Meet Event Discovery** – Users can browse available car meet events and view details such as title, location, date, organizer, and description.
- 📝 **Event Registration** – Users can sign up for events by entering personal details and car information.
- 🏎 **Car Details Selection** – Users can choose car brand, model, and generation, with technical details such as horsepower and engine capacity displayed automatically.
- 📸 **Car Photo Upload** – Users can upload up to 5 photos of their car when registering for an event.
- 👤 **User Profile Management** – Users can view and edit profile information, including avatar, phone number, location, social links, and car description.
- 🔐 **Authentication System** – Login is handled through Firebase Authentication using email and password.
- 🧑‍💼 **Role-Based Navigation** – The app displays different pages depending on the user role: regular user or administrator. Use **"participant@email.com"** for user role or **"organizator@admin.com"** for admin role. Password: "123456"
- 🛠 **Admin Event Management** – Admin users can add, edit, delete, and inspect events and participants.
- 📋 **Participant Review** – Admin users can view participants registered for each event and inspect their uploaded car photos.
- 📱 **User-Friendly Interface** – The UI uses simple pages, card-based layouts, forms, lists, and a dark visual theme.

## 🛠️ Tech Stack

- **Framework:** .NET MAUI
- **Language:** C#
- **Frontend/UI:** XAML
- **Navigation:** .NET MAUI Shell / NavigationPage
- **Authentication:** Firebase Authentication REST API
- **Database:** SQLite
- **ORM:** Entity Framework Core
- **Local Storage:** FileSystem.AppDataDirectory for avatar and car image files
- **Data Binding:** BindingContext, INotifyPropertyChanged, ObservableCollection
- **Target Platforms:** Android and Windows

## 🧩 Main Technical Components

- **Pages:** Login, Home, Events, Event Sign Up, My Events, User Profile, Edit Profile, Admin Events, Add Event, Edit Event, Event Details.
- **Models:** User, EventItem, EventUser, UserRole.
- **Services:** DatabaseService, FirebaseAuthService, UserSession, CarDataService.
- **Database Context:** CarMeetDbContext, used for SQLite access through Entity Framework Core.
- **Image Handling:** Profile avatars and car photos are saved locally, while their file paths are stored in the database.

## 🔄 Application Flow

1. The user opens the app and is redirected to the **Login Page**.
2. The app authenticates the user through **Firebase Authentication**.
3. After login, the app builds the navigation menu based on the user's role.
4. A regular user can browse events, register for an event, upload car photos, and edit their profile.
5. An admin user can create, update, delete, and inspect events and participants.
6. Data is stored locally in **SQLite**, while uploaded images are stored in the app's local file system.

## 📁 Project Structure

```text
CarMeetApp/
├── Data/
│   └── CarMeetDbContext.cs
├── Models/
│   ├── User.cs
│   ├── EventItem.cs
│   ├── EventUser.cs
│   └── UserRole.cs
├── Pages/
│   ├── LoginPage.xaml
│   ├── HomePage.xaml
│   ├── EventsPage.xaml
│   ├── EventSignUpPage.xaml
│   ├── UserProfilePage.xaml
│   └── Admin pages
├── Services/
│   ├── DatabaseService.cs
│   ├── FirebaseAuthService.cs
│   ├── UserSession.cs
│   └── CarDataService.cs
├── App.xaml
├── AppShell.xaml
└── MauiProgram.cs
```

