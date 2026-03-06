import { useState, useEffect, useCallback } from 'react'
import api from '../services/api'

export default function EmployeeList() {
  const [employees, setEmployees] = useState([])
  const [search, setSearch] = useState('')
  const [loading, setLoading] = useState(false)

  const fetchEmployees = useCallback(async (query) => {
    setLoading(true)
    try {
      const response = await api.get('/employees/search', { params: { q: query } })
      setEmployees(response.data)
    } catch (error) {
      console.error('Error fetching employees', error)
    } finally {
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    const timer = setTimeout(() => {
      fetchEmployees(search)
    }, 500)
    return () => clearTimeout(timer)
  }, [search, fetchEmployees])

  const handleDelete = async (id) => {
    if (!confirm('Are you sure?')) return
    try {
      await api.delete(`/employees/${id}`)
      fetchEmployees(search)
    } catch (error) {
      alert('Delete failed')
    }
  }

  const downloadPdf = async (id) => {
    try {
      const response = await api.get(`/employees/${id}/pdf`, { responseType: 'blob' })
      const url = window.URL.createObjectURL(new Blob([response.data]))
      const link = document.createElement('a')
      link.href = url
      link.setAttribute('download', `employee_${id}.pdf`)
      document.body.appendChild(link)
      link.click()
      link.remove()
    } catch (error) {
      alert('PDF download failed')
    }
  }

  return (
    <div>
      <h1 className="text-2xl font-bold mb-4">Employees</h1>
      <input
        type="text"
        placeholder="Search by name, NID, department..."
        className="w-full p-2 border rounded mb-4"
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />
      {loading ? (
        <p>Loading...</p>
      ) : (
        <div className="bg-white shadow overflow-hidden sm:rounded-md">
          <ul className="divide-y divide-gray-200">
            {employees.map((emp) => (
              <li key={emp.id} className="px-6 py-4 flex items-center justify-between">
                <div>
                  <p className="font-medium">{emp.name}</p>
                  <p className="text-sm text-gray-500">NID: {emp.nid}</p>
                  <p className="text-sm text-gray-500">Dept: {emp.department}</p>
                </div>
                <div className="flex space-x-2">
                  <button
                    onClick={() => downloadPdf(emp.id)}
                    className="bg-green-500 text-white px-3 py-1 rounded hover:bg-green-600"
                  >
                    PDF
                  </button>
                  <button
                    onClick={() => handleDelete(emp.id)}
                    className="bg-red-500 text-white px-3 py-1 rounded hover:bg-red-600"
                  >
                    Delete
                  </button>
                </div>
              </li>
            ))}
            {employees.length === 0 && <li className="px-6 py-4 text-gray-500">No employees found.</li>}
          </ul>
        </div>
      )}
    </div>
  )
}
