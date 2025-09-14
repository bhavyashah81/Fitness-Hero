import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { jsPDF } from 'jspdf';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  tasks: { name: string; category: string; duration: number; date: string; completed: boolean }[] = [];
  workoutHistory: string[] = [];
  motivationalQuote: string = '';
  weeklyGoal: number = 0;

  quotes: string[] = [
    'Push yourself, because no one else is going to do it for you.',
    'Great things never come from comfort zones.',
    'Success doesn’t just find you. You have to go out and get it.',
    'The harder you work for something, the greater you’ll feel when you achieve it.',
    'Dream it. Wish it. Do it.',
    'Don’t stop when you’re tired. Stop when you’re done.',
    'It’s going to be hard, but hard does not mean impossible.',
    'Work hard in silence, let success make the noise.',
  ];

  constructor() {
    this.generateRandomQuote(); // Display a quote when the app starts
  }

  addWorkout() {
    const workoutInput = document.getElementById('taskInput') as HTMLInputElement;
    const categoryInput = document.getElementById('categoryInput') as HTMLSelectElement;
    const durationInput = document.getElementById('durationInput') as HTMLInputElement;
    const dateInput = document.getElementById('dateInput') as HTMLInputElement;

    if (workoutInput.value.trim() && durationInput.valueAsNumber > 0 && dateInput.value) {
      const workout = {
        name: workoutInput.value.trim(),
        category: categoryInput.value,
        duration: durationInput.valueAsNumber,
        date: dateInput.value,
        completed: false,
      };

      this.tasks.push(workout);
      this.workoutHistory.push(`${workout.name} (${workout.category}, ${workout.duration} mins, ${workout.date})`);

      workoutInput.value = '';
      categoryInput.value = 'Cardio';
      durationInput.value = '';
      dateInput.value = '';

      this.renderTaskList();
      this.renderHistoryList();
      this.updateTotalDuration();
    }
  }

  renderTaskList() {
    const ulElement = document.getElementById('taskList');
    if (ulElement) {
      ulElement.innerHTML = '';
      this.tasks.forEach((task, index) => {
        const li = document.createElement('li');
        li.className = 'task-item';

        const checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.checked = task.completed;
        checkbox.onchange = () => {
          task.completed = checkbox.checked;
          li.style.textDecoration = task.completed ? 'line-through' : 'none';
        };

        const taskName = document.createElement('span');
        taskName.className = 'task-name';
        taskName.textContent = `${task.name} (${task.category}, ${task.duration} mins, ${task.date})`;

        const removeButton = document.createElement('button');
        removeButton.textContent = 'Remove';
        removeButton.className = 'remove-btn';
        removeButton.onclick = () => this.removeTask(index);

        li.appendChild(checkbox);
        li.appendChild(taskName);
        li.appendChild(removeButton);

        ulElement.appendChild(li);
      });
    }
  }

  renderHistoryList() {
    const ulElement = document.getElementById('historyList');
    if (ulElement) {
      ulElement.innerHTML = '';
      this.workoutHistory.forEach((entry) => {
        const li = document.createElement('li');
        li.textContent = entry;
        ulElement.appendChild(li);
      });
    }
  }

  generateRandomQuote() {
    const randomIndex = Math.floor(Math.random() * this.quotes.length);
    this.motivationalQuote = this.quotes[randomIndex];
  }

  removeTask(index: number) {
    this.tasks.splice(index, 1);
    this.renderTaskList();
    this.updateTotalDuration();
  }

  updateTotalDuration() {
    const totalDurationElement = document.getElementById('totalDuration');
    if (totalDurationElement) {
      totalDurationElement.textContent = this.getTotalDuration().toString();
    }
  }

  getTotalDuration() {
    return this.tasks.reduce((total, task) => total + task.duration, 0);
  }

  setWeeklyGoal() {
    const goalInput = document.getElementById('goalInput') as HTMLInputElement;
    this.weeklyGoal = goalInput.valueAsNumber || 0;
    goalInput.value = '';
  }

  getProgress() {
    const totalDuration = this.getTotalDuration();
    return this.weeklyGoal > 0 ? (totalDuration / this.weeklyGoal) * 100 : 0;
  }

  getCategoryCount(category: string) {
    return this.tasks.filter((task) => task.category === category).length;
  }

  getCategoryIcon(category: string): string {
    switch (category) {
      case 'Cardio':
        return 'directions_run';
      case 'Strength':
        return 'fitness_center';
      case 'Flexibility':
        return 'self_improvement';
      default:
        return 'fitness_center';
    }
  }

  // Generate PDF for workout statistics
  generatePDF() {
    const doc = new jsPDF();

    // Title
    doc.setFontSize(20);
    doc.text('Workout App Statistics', 20, 20);

    // Total Duration
    doc.setFontSize(14);
    doc.text(`Total Duration: ${this.getTotalDuration()} mins`, 20, 40);

    // Weekly Goal
    doc.text(`Weekly Goal: ${this.weeklyGoal} mins`, 20, 50);

    // Workout Analytics
    doc.text('Workout Analytics:', 20, 70);
    doc.text(`- Cardio Workouts: ${this.getCategoryCount('Cardio')}`, 30, 80);
    doc.text(`- Strength Workouts: ${this.getCategoryCount('Strength')}`, 30, 90);
    doc.text(`- Flexibility Workouts: ${this.getCategoryCount('Flexibility')}`, 30, 100);

    // Workout History
    doc.text('Workout History:', 20, 120);
    this.workoutHistory.forEach((history, index) => {
      doc.text(`- ${history}`, 30, 130 + index * 10);
    });

    // Save PDF
    doc.save('Workout_Statistics.pdf');
  }
}
