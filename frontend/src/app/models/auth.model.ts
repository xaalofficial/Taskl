export interface LoginRequest {
    email: string;
    password: string;
  }
  
  export interface LoginResponse {
    token: string;
    email: string;
    fullName: string;
  }
  
  export interface User {
    email: string;
    fullName: string;
  }