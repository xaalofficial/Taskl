export interface Task {
    id: string;
    title: string;
    description?: string;
    dueDate?: string;
    isCompleted: boolean;
    createdAt: string;
    completedAt?: string;
  }
  
  export interface CreateTaskRequest {
    title: string;
    description?: string;
    dueDate?: string;
  }
  
  export interface UpdateTaskRequest {
    title: string;
    description?: string;
    dueDate?: string;
  }
  
  export interface PagedResult<T> {
    items: T[];
    totalCount: number;
    pageNumber: number;
    pageSize: number;
  }
  
  export interface TaskQueryParams {
    searchTerm?: string;
    isCompleted?: boolean;
    pageNumber?: number;
    pageSize?: number;
  }