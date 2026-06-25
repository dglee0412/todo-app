import { useState } from "react"
import { useNavigate } from 'react-router-dom';
import apiClient from "../api/client"
import type { AuthResponse } from "../api/types"

function LoginPage()
{
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const navigate = useNavigate();

    const handleLogin = async () =>
    {
        try
        {
            const response = await apiClient.post<AuthResponse>('/auth/login',
                {
                    email: email,
                    password: password,
                }
            )

            //성공 - 응답에서 토큰 깨너기
            localStorage.setItem('token', response.data.token);            
            navigate('/todos');
        }
        catch (error)
        {
            console.error('로그인 실패: ', error);
            alert('로그인실패');
        }
        
    }
    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100">
            <div className="bg-white p-8 rounded-lg shadow-md w-96">
                <h1 className="text-2xl font-bold mb-6 text-center">로그인</h1>
                
                <input 
                    type="text" 
                    placeholder="이메일" 
                    value={email} 
                    onChange={(e) => setEmail(e.target.value)} 
                    className="w-full border border-gray-300 rounded px-3 py-2 mb-4" />

                <input
                    type="password" 
                    placeholder="비밀번호" 
                    value={password} 
                    onChange={(e) => setPassword(e.target.value)} 
                    className="w-full border border-gray-300 rounded px-3 py-2 mb-4" />

                <button
                    onClick={handleLogin}
                    className="w-full bg-blue-600 text-white rounded px-3 py-2 hover:bg-blue-700">로그인
                </button>
            </div>
        </div>
    )
}

export default LoginPage