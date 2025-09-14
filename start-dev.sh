#!/bin/bash

# Fitness Hero Development Startup Script
echo "🏋️ Starting Fitness Hero Development Environment..."

# Check if Node.js is installed
if ! command -v node &> /dev/null; then
    echo "❌ Node.js is not installed. Please install Node.js first."
    exit 1
fi

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK is not installed. Please install .NET SDK first."
    exit 1
fi

# Install frontend dependencies if needed
echo "📦 Installing frontend dependencies..."
cd Fitness_CL
if [ ! -d "node_modules" ]; then
    npm install
fi

# Start backend in background
echo "🚀 Starting .NET Backend on http://localhost:5001..."
cd ../Fitness_SE
dotnet run --urls "http://localhost:5001" &
BACKEND_PID=$!

# Wait a moment for backend to start
sleep 3

# Start frontend
echo "🎨 Starting Angular Frontend on http://localhost:4200..."
cd ../Fitness_CL
npm start &
FRONTEND_PID=$!

echo ""
echo "✅ Fitness Hero is starting up!"
echo "🌐 Frontend: http://localhost:4200"
echo "🔧 Backend API: http://localhost:5002"
echo "📊 Weather API: http://localhost:5002/weatherforecast"
echo ""
echo "Press Ctrl+C to stop all services..."

# Function to cleanup processes on exit
cleanup() {
    echo ""
    echo "🛑 Stopping services..."
    kill $BACKEND_PID 2>/dev/null
    kill $FRONTEND_PID 2>/dev/null
    exit 0
}

# Set trap to cleanup on script exit
trap cleanup SIGINT SIGTERM

# Wait for processes
wait


