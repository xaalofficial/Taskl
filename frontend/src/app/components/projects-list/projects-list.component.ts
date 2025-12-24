import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProjectService } from '../../services/project.service';
import { TaskService } from '../../services/task.service';
import { ToastService } from '../../services/toast.service';
import { Project } from '../../models/project.model';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

interface ProjectWithProgress extends Project {
  completedTasks: number;
  totalTasks: number;
  progressPercentage: number;
}

@Component({
  selector: 'app-projects-list',
  templateUrl: './projects-list.component.html',
  styleUrls: ['./projects-list.component.css']
})
export class ProjectsListComponent implements OnInit {
  projects: ProjectWithProgress[] = [];
  isLoading = false;

  constructor(
    private projectService: ProjectService,
    private taskService: TaskService,
    private router: Router,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.isLoading = true;
    this.projectService.getProjects().subscribe({
      next: (projects) => {
        const projectRequests = projects.map(project =>
          this.taskService.getTasks(project.id, { pageSize: 1000 }).pipe(
            catchError(() => of({ items: [], totalCount: 0, pageNumber: 1, pageSize: 1000 }))
          )
        );

        forkJoin(projectRequests).subscribe({
          next: (taskResults) => {
            this.projects = projects.map((project, index) => {
              const tasks = taskResults[index].items;
              const totalTasks = tasks.length;
              const completedTasks = tasks.filter(t => t.isCompleted).length;
              const progressPercentage = totalTasks > 0 ? Math.round((completedTasks / totalTasks) * 100) : 0;

              return {
                ...project,
                completedTasks,
                totalTasks,
                progressPercentage
              };
            });
            this.isLoading = false;
          },
          error: () => {
            this.isLoading = false;
          }
        });
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  createProject(): void {
    this.router.navigate(['/projects/new']);
  }

  viewProject(id: string): void {
    this.router.navigate(['/projects', id]);
  }
}