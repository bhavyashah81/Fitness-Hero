# Fitness Hero
A workout tracking application with motivational quotes, analytics, and PDF reporting capabilities

## Features
- **Workout Logging**: Add workouts with categories (Cardio, Strength, Flexibility), duration, and dates
- **Motivational Quotes**: Built-in collection of inspiring fitness quotes with random quote generation
- **Workout History**: Complete log of all completed workouts with detailed tracking
- **Workout Analytics**: Real-time statistics showing workout counts by category (Cardio, Strength, Flexibility)
- **PDF Export**: Generate and download workout statistics as PDF reports using jsPDF integration
- **External Quote API**: Integration with RealInspire API for additional motivational content
- **Responsive Design**: Clean, card-based UI that works across all device sizes
- **Navigation**: Multi-page application with Home and About sections using Angular Router
- **Weather Data**: Backend provides sample weather forecast data

## Technologies and Languages Used
**Frontend:**
- Languages: TypeScript, HTML, CSS
- Framework: Angular 19
- UI Components: Angular Material
- Additional Libraries: jsPDF, RxJS, Express

**Backend:**
- Language: C#
- Framework: .NET 9.0 Minimal API
- Documentation: OpenAPI/Swagger integration

**APIs:**
- RealInspire API (motivational quotes)
- Weather forecast endpoint (sample data)

## How to Run the Project
```bash
./start-dev.sh
```

This will:
- Check for Node.js and .NET SDK prerequisites  
- Install Angular dependencies automatically
- Launch the .NET backend API
- Launch the Angular frontend
- Enable workout logging and tracking functionality
- Provide access to motivational quotes and analytics
- Support PDF generation for workout statistics
- Serve sample weather forecast data

## Prerequisites
Install required software:
- **Node.js** (v18 or higher) 
- **.NET SDK** (v9.0 or higher)

## Dependencies

**Frontend Dependencies:**
```json
"@angular/core": "^19.0.0"
"@angular/material": "^19.0.5"
"@angular/forms": "^19.0.6"
"@angular/router": "^19.0.0"
"@angular/ssr": "^19.0.7"
"jspdf": "^2.5.2"
"rxjs": "~7.8.0"
"express": "^4.18.2"
```

**Backend Dependencies:**
```xml
Microsoft.AspNetCore.OpenApi (9.0.0)
```