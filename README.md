# 🗳️ Online Polling System

## 📋 Project Overview

Online Polling System is a comprehensive, full-stack application designed to create, manage, and participate in polls across multiple platforms.

### 🚀 Key Features
- Web-based polling platform
- Mobile polling application
- Real-time vote tracking
- User authentication
- Detailed poll analytics

## 🏗️ System Architecture

### Components
- 🖥️ **Backend**: ASP.NET Core Web API
- 🌐 **Frontend**: React.js
- 📱 **Mobile**: .NET MAUI
- 💾 **Database**: SQL Server
- 🔐 **Authentication**: JWT


<details>
/\ | | __ | | __ | |_ | | ___ | |__ () __ ___ //\| |/  | '_ \ / _ | ' | |/ _ | ' | | '_ / | / _ \ | (| | | | | (| | | | | | () | | | | | | | _
_/ _/|_,|| ||_,|| |||_/|| |||| ||__/
</details>

## 📦 Prerequisites

### Development Environment
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Node.js 16+](https://nodejs.org/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## 🛠️ Setup and Installation

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/online-polling-system.git
cd online-polling-system
```

### 2. Setup the Backend
```bash
cd src/backend
dotnet restore
dotnet ef database update
dotnet run
```

### 3. Setup the Frontend
```bash
cd src/frontend
npm install
npm start
``` 

### 4. Setup the Mobile App
```bash
cd src/mobile
dotnet restore
dotnet build
dotnet run
```

### Configuration
- Update the database connection string in `appsettings.json`
 ```bash
 {
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PollSystemDB;"
  },
  "Jwt": {
    "Key": "YourSecretKeyHere",
    "Issuer": "OnlinePollSystemAPI",
    "Audience": "OnlinePollSystemClient"
  }
}
```

### Frontend Configuration
- Update the API URL in `src/frontend/src/services/api.js`
```bash
REACT_APP_API_URL=http://localhost:5000/api
```

