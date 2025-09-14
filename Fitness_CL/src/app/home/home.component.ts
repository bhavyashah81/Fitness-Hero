import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatMenuModule } from '@angular/material/menu';
import { MatBadgeModule } from '@angular/material/badge';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDividerModule } from '@angular/material/divider';
import { jsPDF } from 'jspdf';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatToolbarModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatListModule,
    MatSidenavModule,
    MatMenuModule,
    MatBadgeModule,
    MatChipsModule,
    MatProgressBarModule,
    MatDividerModule
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  workoutName: string = '';
  workoutCategory: string = '';
  workoutDuration: number = 0;
  motivationalQuote: string = '';
  
  tasks: { name: string; category: string; duration: number; date: string; completed: boolean }[] = [];
  workoutHistory: string[] = [];
  sidenavOpened: boolean = false;

  // Navigation items
  navigationItems = [
    { name: 'Dashboard', icon: 'dashboard', active: true },
    { name: 'Workouts', icon: 'fitness_center', active: false },
    { name: 'Progress', icon: 'trending_up', active: false },
    { name: 'Nutrition', icon: 'restaurant', active: false },
    { name: 'Goals', icon: 'flag', active: false },
    { name: 'Community', icon: 'people', active: false },
    { name: 'Settings', icon: 'settings', active: false }
  ];

  // User info
  userInfo = {
    name: 'Fitness Hero',
    level: 'Beginner',
    avatar: 'FH',
    notifications: 3
  };

  quotes: string[] = [
    "The only bad workout is the one that didn't happen.",
    "Your body can do it. It's your mind you need to convince.",
    "Don't wish for it, work for it.",
    "Success starts with self-discipline.",
    "Make yourself proud.",
    "The pain you feel today will be the strength you feel tomorrow.",
    "Champions don't become champions in the ring. They become champions in their training.",
    "If you want something you've never had, you must be willing to do something you've never done."
  ];

  constructor() {
    this.generateRandomQuote();
  }

  generateRandomQuote() {
    const randomIndex = Math.floor(Math.random() * this.quotes.length);
    this.motivationalQuote = this.quotes[randomIndex];
  }

  addWorkout() {
    if (this.workoutName && this.workoutCategory && this.workoutDuration > 0) {
      const newTask = {
        name: this.workoutName,
        category: this.workoutCategory,
        duration: this.workoutDuration,
        date: new Date().toLocaleDateString(),
        completed: true
      };
      
      this.tasks.push(newTask);
      this.workoutHistory.push(`${this.workoutName} - ${this.workoutDuration} minutes`);
      
      // Reset form
      this.workoutName = '';
      this.workoutCategory = '';
      this.workoutDuration = 0;
      
      alert('Workout added successfully! 💪');
    } else {
      alert('Please fill in all fields with valid data.');
    }
  }

  getCategoryCount(category: string): number {
    return this.tasks.filter(task => task.category === category).length;
  }

  generatePDF() {
    if (this.tasks.length === 0) {
      alert('No workout data to export. Add some workouts first!');
      return;
    }

    const doc = new jsPDF();
    
    // Title
    doc.setFontSize(20);
    doc.text('Fitness Hero - Workout Report', 20, 20);
    
    // Date
    doc.setFontSize(12);
    doc.text(`Generated on: ${new Date().toLocaleDateString()}`, 20, 35);
    
    // Stats
    doc.setFontSize(14);
    doc.text('Workout Statistics:', 20, 50);
    doc.setFontSize(12);
    doc.text(`Total Workouts: ${this.tasks.length}`, 25, 65);
    doc.text(`Cardio: ${this.getCategoryCount('Cardio')}`, 25, 75);
    doc.text(`Strength: ${this.getCategoryCount('Strength')}`, 25, 85);
    doc.text(`Flexibility: ${this.getCategoryCount('Flexibility')}`, 25, 95);
    doc.text(`Sports: ${this.getCategoryCount('Sports')}`, 25, 105);
    
    // Workout List
    doc.setFontSize(14);
    doc.text('Workout History:', 20, 125);
    
    let yPosition = 140;
    this.tasks.forEach((task, index) => {
      if (yPosition > 270) {
        doc.addPage();
        yPosition = 20;
      }
      
      doc.setFontSize(10);
      doc.text(`${index + 1}. ${task.name} (${task.category}) - ${task.duration} min - ${task.date}`, 25, yPosition);
      yPosition += 10;
    });
    
    // Save the PDF
    doc.save('fitness-hero-workout-report.pdf');
    alert('PDF downloaded successfully! 📄');
  }

  // Progress Charts Methods
  getWeeklyWorkouts(): number {
    return this.tasks.filter(task => task.completed).length;
  }

  getTotalMinutes(): number {
    return this.tasks.reduce((total, task) => total + task.duration, 0);
  }

  getStreak(): number {
    return 7; // Mock streak
  }

  getWeeklyProgress(): number {
    return Math.min((this.getWeeklyWorkouts() / 5) * 100, 100);
  }

  viewDetailedStats() {
    alert('Stats: ' + this.tasks.length + ' workouts completed!');
  }

  // Goals & Achievements Methods
  getRecentAchievements(): any[] {
    return [
      { name: 'First Workout', date: '2025-09-10', icon: 'star' },
      { name: 'Week Streak', date: '2025-09-12', icon: 'local_fire_department' }
    ];
  }

  getCurrentGoal(): string {
    return 'Complete 5 workouts this week (' + this.tasks.length + '/5)';
  }

  setNewGoal() {
    alert('Goal setting coming soon!');
  }

  // Navigation methods
  toggleSidenav() {
    this.sidenavOpened = !this.sidenavOpened;
  }

  navigateTo(item: any) {
    alert('Navigating to: ' + item.name);
    this.navigationItems.forEach(nav => nav.active = false);
    item.active = true;
  }

  // User actions
  openUserProfile() {
    alert('User profile coming soon!');
  }

  openNotifications() {
    alert('You have ' + this.userInfo.notifications + ' notifications!');
  }

  openSettings() {
    alert('Settings coming soon!');
  }

  // Quick actions
  quickStartWorkout() {
    alert('Quick workout started! 💪');
  }

  viewAllWorkouts() {
    if (this.tasks.length === 0) {
      alert('No workouts yet. Add your first workout!');
    } else {
      alert('Workouts:\n' + this.tasks.map(task => 
        `${task.name} (${task.category}) - ${task.duration} min - ${task.date}`
      ).join('\n'));
    }
  }
}