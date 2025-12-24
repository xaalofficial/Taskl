import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent implements OnInit {
  taskForm: FormGroup;
  isEditMode = false;
  projectId: string | null = null;
  taskId: string | null = null;
  isLoading = false;
  isSaving = false;

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private router: Router,
    private route: ActivatedRoute,
    private toastService: ToastService
  ) {
    this.taskForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.maxLength(1000)]],
      dueDate: ['']
    });
  }

  ngOnInit(): void {
    this.projectId = this.route.snapshot.paramMap.get('projectId');
    this.taskId = this.route.snapshot.paramMap.get('taskId');
    
    if (!this.projectId) {
      this.router.navigate(['/projects']);
      return;
    }

    if (this.taskId) {
      this.isEditMode = true;
      this.loadTask();
    }
  }

  loadTask(): void {
    if (!this.projectId || !this.taskId) return;

    this.isLoading = true;
    this.taskService.getTask(this.projectId, this.taskId).subscribe({
      next: (task) => {
        this.taskForm.patchValue({
          title: task.title,
          description: task.description || '',
          dueDate: task.dueDate ? this.formatDateForInput(task.dueDate) : ''
        });
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.router.navigate(['/projects', this.projectId]);
      }
    });
  }

  formatDateForInput(dateString: string): string {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  onSubmit(): void {
    if (this.taskForm.invalid || !this.projectId) {
      this.taskForm.markAllAsTouched();
      return;
    }

    this.isSaving = true;
    const formValue = { ...this.taskForm.value };
    
    // Convert date to ISO string if provided
    if (formValue.dueDate) {
      formValue.dueDate = new Date(formValue.dueDate).toISOString();
    } else {
      formValue.dueDate = null;
    }

    if (this.isEditMode && this.taskId) {
      this.taskService.updateTask(this.projectId, this.taskId, formValue).subscribe({
        next: () => {
          this.toastService.success('Task updated successfully!');
          this.router.navigate(['/projects', this.projectId]);
        },
        error: () => {
          this.isSaving = false;
        }
      });
    } else {
      this.taskService.createTask(this.projectId, formValue).subscribe({
        next: () => {
          this.toastService.success('Task created successfully!');
          this.router.navigate(['/projects', this.projectId]);
        },
        error: () => {
          this.isSaving = false;
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/projects', this.projectId]);
  }

  get title() {
    return this.taskForm.get('title');
  }

  get description() {
    return this.taskForm.get('description');
  }

  get dueDate() {
    return this.taskForm.get('dueDate');
  }
}
