export interface User{
    id?: number,
    username: string,
    email: string,
    role: string,
    firstname: string,
    lastname: string
}
export interface UserResponse {
  message: string;
  data: User[];
}