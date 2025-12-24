import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task, CreateTaskRequest, UpdateTaskRequest, PagedResult, TaskQueryParams } from '../models/task.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  constructor(private http: HttpClient) {}

  getTasks(projectId: string, params?: TaskQueryParams): Observable<PagedResult<Task>> {
    let httpParams = new HttpParams();
    
    if (params?.searchTerm) {
      httpParams = httpParams.set('searchTerm', params.searchTerm);
    }
    if (params?.isCompleted !== undefined) {
      httpParams = httpParams.set('isCompleted', params.isCompleted.toString());
    }
    if (params?.pageNumber) {
      httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
    }
    if (params?.pageSize) {
      httpParams = httpParams.set('pageSize', params.pageSize.toString());
    }

    return this.http.get<PagedResult<Task>>(`${environment.apiUrl}/projects/${projectId}/tasks`, { params: httpParams });
  }

  getTask(projectId: string, taskId: string): Observable<Task> {
    return this.http.get<Task>(`${environment.apiUrl}/projects/${projectId}/tasks/${taskId}`);
  }

  createTask(projectId: string, request: CreateTaskRequest): Observable<Task> {
    return this.http.post<Task>(`${environment.apiUrl}/projects/${projectId}/tasks`, request);
  }

  updateTask(projectId: string, taskId: string, request: UpdateTaskRequest): Observable<Task> {
    return this.http.put<Task>(`${environment.apiUrl}/projects/${projectId}/tasks/${taskId}`, request);
  }

  deleteTask(projectId: string, taskId: string): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/projects/${projectId}/tasks/${taskId}`);
  }

  completeTask(projectId: string, taskId: string): Observable<void> {
    return this.http.patch<void>(`${environment.apiUrl}/projects/${projectId}/tasks/${taskId}/complete`, {});
  }
}