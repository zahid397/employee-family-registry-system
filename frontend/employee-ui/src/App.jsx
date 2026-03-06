import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import EmployeeList from './components/EmployeeList'
import AddEmployee from './components/AddEmployee'

function App() {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gray-100">
        <nav className="bg-white shadow-sm">
          <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
            <div className="flex justify-between h-16">
              <div className="flex">
                <Link to="/" className="flex items-center px-3 text-gray-900 font-semibold">
                  Employee Registry
                </Link>
                <div className="ml-6 flex space-x-4">
                  <Link to="/" className="inline-flex items-center px-3 py-2 text-gray-700 hover:text-gray-900">
                    Employees
                  </Link>
                  <Link to="/add" className="inline-flex items-center px-3 py-2 text-gray-700 hover:text-gray-900">
                    Add Employee
                  </Link>
                </div>
              </div>
            </div>
          </div>
        </nav>
        <div className="max-w-7xl mx-auto py-6 sm:px-6 lg:px-8">
          <Routes>
            <Route path="/" element={<EmployeeList />} />
            <Route path="/add" element={<AddEmployee />} />
          </Routes>
        </div>
      </div>
    </BrowserRouter>
  )
}

export default App
