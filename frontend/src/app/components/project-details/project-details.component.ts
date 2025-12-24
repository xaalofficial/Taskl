
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProjectService } from '../../services/project.service';
import { TaskService } from '../../services/task.service';
import { ToastService } from '../../services/toast.service';
import { Project } from '../../models/project.model';
import { Task, PagedResult } from '../../models/task.model';

@Component({
  selector: 'app-project-details',
  templateUrl: './project-details.component.html',
  styleUrls: ['./project-details.component.css']
})
export class ProjectDetailsComponent implements OnInit {
  project: Project | null = null;
  tasks: Task[] = [];
  totalCount = 0;
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;
  
  searchTerm = '';
  filterCompleted: boolean | null = null;
  
  isLoading = false;
  isLoadingTasks = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private projectService: ProjectService,
    private taskService: TaskService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    const projectId = this.route.snapshot.paramMap.get('id');
    if (projectId) {
      this.loadProject(projectId);
      this.loadTasks(projectId);
    }
  }

  loadProject(projectId: string): void {
    this.isLoading = true;
    this.projectService.getProject(projectId).subscribe({
      next: (project) => {
        this.project = project;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.router.navigate(['/projects']);
      }
    });
  }

  loadTasks(projectId: string): void {
    this.isLoadingTasks = true;
    
    const params: any = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize
    };

    if (this.searchTerm) {
      params.searchTerm = this.searchTerm;
    }
    if (this.filterCompleted !== null) {
      params.isCompleted = this.filterCompleted;
    }

    this.taskService.getTasks(projectId, params).subscribe({
      next: (result: PagedResult<Task>) => {
        this.tasks = result.items;
        this.totalCount = result.totalCount;
        this.totalPages = Math.ceil(this.totalCount / this.pageSize);
        this.isLoadingTasks = false;
      },
      error: () => {
        this.isLoadingTasks = false;
      }
    });
  }

  onSearch(): void {
    this.currentPage = 1;
    if (this.project) {
      this.loadTasks(this.project.id);
    }
  }

  onFilterChange(value: string): void {
    if (value === 'all') {
      this.filterCompleted = null;
    } else if (value === 'completed') {
      this.filterCompleted = true;
    } else if (value === 'pending') {
      this.filterCompleted = false;
    }
    this.currentPage = 1;
    if (this.project) {
      this.loadTasks(this.project.id);
    }
  }

  goToPage(page: number): void {
    this.currentPage = page;
    if (this.project) {
      this.loadTasks(this.project.id);
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.goToPage(this.currentPage - 1);
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.goToPage(this.currentPage + 1);
    }
  }

  getProgressPercentage(): number {
    if (this.totalCount === 0) return 0;
    const completedCount = this.tasks.filter(t => t.isCompleted).length;
    return Math.round((completedCount / this.totalCount) * 100);
  }

  editProject(): void {
    if (this.project) {
      this.router.navigate(['/projects', this.project.id, 'edit']);
    }
  }

  deleteProject(): void {
    if (!this.project) return;
    
    if (confirm(`Are you sure you want to delete "${this.project.title}"? This will also delete all tasks.`)) {
      this.projectService.deleteProject(this.project.id).subscribe({
        next: () => {
          this.toastService.success('Project deleted successfully!');
          this.router.navigate(['/projects']);
        }
      });
    }
  }

  addTask(): void {
    if (this.project) {
      this.router.navigate(['/projects', this.project.id, 'tasks', 'new']);
    }
  }

  editTask(taskId: string): void {
    if (this.project) {
      this.router.navigate(['/projects', this.project.id, 'tasks', taskId, 'edit']);
    }
  }

  toggleTaskCompletion(task: Task): void {
    if (!this.project) return;
    
    if (task.isCompleted) {
      // Already completed, do nothing (checkbox is disabled)
      return;
    }
    
    // Mark as complete
    this.taskService.completeTask(this.project.id, task.id).subscribe({
      next: () => {
        this.toastService.success('Task marked as complete!');
        this.loadTasks(this.project!.id);
      }
    });
  }

  deleteTask(task: Task): void {
    if (!this.project) return;
    
    if (confirm(`Are you sure you want to delete "${task.title}"?`)) {
      this.taskService.deleteTask(this.project.id, task.id).subscribe({
        next: () => {
          this.toastService.success('Task deleted successfully!');
          this.loadTasks(this.project!.id);
        }
      });
    }
  }

  isOverdue(dueDate?: string): boolean {
    if (!dueDate) return false;
    return new Date(dueDate) < new Date() && !this.tasks.find(t => t.dueDate === dueDate)?.isCompleted;
  }

  formatDate(dateString?: string): string {
    if (!dateString) return 'No due date';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
  }
}