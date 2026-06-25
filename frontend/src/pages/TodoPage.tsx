import {useState, useEffect} from 'react'
import { useNavigate } from 'react-router-dom';
import apiClient from '../api/client'
import type {Todo} from '../api/types'


function TodoPage()
{
    const [todos, setTodos] = useState<Todo[]>([]); //Todo 목록 상태
    const [newTitle, setNewTitle] = useState('');
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem('token');
        navigate('/login');
    }

    const fetchTodos = async () => {
        try
        {
            const response = await apiClient.get<Todo[]>('/todos');
            setTodos(response.data);
        }
        catch(error)
        {
            console.error('목록 불러오기 실패: ', error);
        }
    };    

    //화면이 처음 뜰 때 Todo 목록을 불러옴
    useEffect(() =>{        
        fetchTodos()
    }, []) // 빈배열 = 처음 한번만

    //Todo 생성
    const handleCreate = async() => {
        if(!newTitle.trim()) return; //빈 제목이면 무시

        try{
            await apiClient.post('/todos', {
                title: newTitle,
                description: null,
                priority: 'Medium',
                dueDate: null,
            });
            setNewTitle(''); //입력칸 비우기
            fetchTodos(); //목록 다시 불러오기 (방금 만든게 보이게)
        }
        catch(error){
            console.error('생성 실패: ', error);
        }
    }

    //완료 상태 토글 (수정)
    const handleToggle = async (todo: Todo) => {
        try{
            const newStatus = todo.status === 'Completed' ? 'NotStarted' : 'Completed'
            await apiClient.put(`/todos/${todo.id}`, {
                title: todo.title,
                descripton: todo.description,
                status: newStatus,
                priority: todo.priority,
                dueDate: todo.dueDate,
            })
            fetchTodos()
        }
        catch(error){
            console.error('수정 실패: ', error);
        }
    }

    //삭제
    const handleDelete = async (id: number) => {
        try {
            await apiClient.delete(`/todos/${id}`)
            fetchTodos()
        }
        catch(error){
            console.error('삭제 실패: ', error);
        }
    }

    return(
        <div className='max-w-2xl mx-auto p-8'>
            <div className='flex items-center justify-between mb-6'>
                <h1 className='text-2xl font-bold mb-6'>내 할 일</h1>
                <button
                    onClick={handleLogout}
                    className='text-sm text-gray-500 hover:text-gray-700'
                >
                    로그아웃
                </button>
            </div>
            {/* 입력 + 추가 버튼 */}
            <div className='flex gap-2 mb-6'>
                <input
                    type='text'
                    placeholder='할 일을 입력하세요'
                    value={newTitle}
                    onChange={(e) => setNewTitle(e.target.value)}
                    className='flex-1 border border-gray-300 rounded px-3 py2'
                />
                <button 
                    onClick={handleCreate}
                    className='bg-blue-600 text-white rounded px-4 py-2 hover:bg-blue-700'
                >추가</button>
            </div>

            <ul className="space-y-2">
                {
                    todos.map((todo) => 
                    (
                        <li key={todo.id} className="bg-white p-4 rounded shadow flex items-center justify-between">
                            <div className='flex items-center gap-3'>
                                <input type='checkbox' checked={todo.status === 'Completed'} onChange={() => handleToggle(todo)} className='w-5 h-5' />
                                <span className={todo.status === 'Completed' ? 'line-through text-gray-400' : 'font-medium'}>
                                    {todo.title}
                                </span>
                            </div>

                            <button onClick={() => handleDelete(todo.id)} className='text-red-500 hover:text-red-700 text-sm'>삭제</button>
                        </li>
                    )
                )}
            </ul>

            {todos.length == 0 && (
                <p className='text-gray-400'>할 일이 없습니다.</p>
            )}
        </div>
    )
}

export default TodoPage