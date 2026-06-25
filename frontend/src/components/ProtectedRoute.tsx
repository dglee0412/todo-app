import { Navigate } from 'react-router-dom'
import type { ReactNode } from 'react'

interface ProtectedRouteProps{
    children: ReactNode
}

function ProtectedRoute({ children }: ProtectedRouteProps){
    const token = localStorage.getItem('token');

    //토큰 없으면 로그인으로 리다이렉트(이동시킴)
    if(!token){
        return <Navigate to='/login' replace />
    }

    //토큰 있으면 원래 보여주려던 화면을 그대로
    return <>{children}</>
}

export default ProtectedRoute