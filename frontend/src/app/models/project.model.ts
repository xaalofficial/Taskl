export interface Project {
    id: string;
    title: string;
    description?: string;
    createdAt: string;
    updatedAt: string;
  }
  
  export interface CreateProjectRequest {
    title: string;
    description?: string;
  }
  
  export interface UpdateProjectRequest {
    title: string;
    description?: string;
  }