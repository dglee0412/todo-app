import axios from 'axios';

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

//요청 interceptor - 모든 요청에 토큰 자동 첨부
apiClient.interceptors.request.use((config) =>
{
    const token = localStorage.getItem('token');
    if(token)
    {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export default apiClient;