//인증 응답(백엔드 AuthResponseDto 와 동일)
export interface AuthResponse{
    userId: number;
    email: string;
    displayName: string;
    token: string;
}

//Todo(백엔드 TodoResponseDto와 동일)
export interface Todo{
    id: number;
    title: string;
    description: string | null;
    status: 'NotStarted' | 'InProgress' | 'Completed';
    priority: 'Low' | 'Medium' | 'High';
    dueDate: string | null;
    createAt: string;
    updateAt: string | null;
}