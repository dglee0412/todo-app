import './App.css'
import {BrowserRouter, Routes, Route, Navigate} from 'react-router-dom'
import LoginPage from './pages/LoginPage'
import TodoPage from './pages/TodoPage'
import ProtectedRoute from './components/ProtectedRoute'


function App() {
    return(
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Navigate to="/login" replace />} />
                <Route path='/login' element={<LoginPage />} />
                <Route 
                    path='/todos' 
                    element={
                        <ProtectedRoute>
                            <TodoPage />
                        </ProtectedRoute>
                    } 
                />
            </Routes>
        </BrowserRouter>
    ) 
}

export default App
