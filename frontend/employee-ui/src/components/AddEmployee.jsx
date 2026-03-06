import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import api from '../services/api'

export default function AddEmployee() {
  const navigate = useNavigate()
  const [form, setForm] = useState({
    name: '',
    nid: '',
    phone: '',
    department: '',
    basicSalary: '',
    spouse: { name: '', nid: '' },
    children: [],
  })
  const [errors, setErrors] = useState({})

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value })
  }

  const handleSpouseChange = (e) => {
    setForm({
      ...form,
      spouse: { ...form.spouse, [e.target.name]: e.target.value },
    })
  }

  const handleChildChange = (index, field, value) => {
    const updated = [...form.children]
    updated[index][field] = value
    setForm({ ...form, children: updated })
  }

  const addChild = () => {
    setForm({
      ...form,
      children: [...form.children, { name: '', dateOfBirth: '' }],
    })
  }

  const removeChild = (index) => {
    const updated = form.children.filter((_, i) => i !== index)
    setForm({ ...form, children: updated })
  }

  const validate = () => {
    const errs = {}
    if (!form.name) errs.name = 'Name required'
    if (!form.nid) errs.nid = 'NID required'
    else if (!/^\d{10}$|^\d{17}$/.test(form.nid)) errs.nid = 'NID must be 10 or 17 digits'
    if (!form.phone) errs.phone = 'Phone required'
    else if (!/^(?:\+8801|01)[3-9]\d{8}$/.test(form.phone)) errs.phone = 'Invalid Bangladeshi phone'
    if (!form.department) errs.department = 'Department required'
    if (!form.basicSalary || parseFloat(form.basicSalary) <= 0) errs.basicSalary = 'Valid salary required'

    if (form.spouse.name || form.spouse.nid) {
      if (!form.spouse.name) errs.spouseName = 'Spouse name required'
      if (!form.spouse.nid) errs.spouseNid = 'Spouse NID required'
      else if (!/^\d{10}$|^\d{17}$/.test(form.spouse.nid)) errs.spouseNid = 'NID must be 10 or 17 digits'
    }

    form.children.forEach((child, idx) => {
      if (!child.name) errs[`childName${idx}`] = 'Child name required'
      if (!child.dateOfBirth) errs[`childDob${idx}`] = 'Date of birth required'
    })

    return errs
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    const errs = validate()
    if (Object.keys(errs).length > 0) {
      setErrors(errs)
      return
    }

    const payload = {
      name: form.name,
      nid: form.nid,
      phone: form.phone,
      department: form.department,
      basicSalary: parseFloat(form.basicSalary),
      spouse: form.spouse.name ? form.spouse : null,
      children: form.children.map(c => ({ name: c.name, dateOfBirth: c.dateOfBirth })),
    }

    try {
      await api.post('/employees', payload)
      navigate('/')
    } catch (error) {
      if (error.response?.data) {
        // FluentValidation errors
        const serverErrors = {}
        error.response.data.forEach(e => serverErrors[e.propertyName] = e.errorMessage)
        setErrors(serverErrors)
      } else {
        alert('Submission failed')
      }
    }
  }

  return (
    <div className="bg-white p-6 rounded shadow">
      <h1 className="text-2xl font-bold mb-4">Add Employee</h1>
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-gray-700">Name *</label>
          <input name="name" value={form.name} onChange={handleChange} className="mt-1 block w-full border rounded p-2" />
          {errors.name && <p className="text-red-500 text-sm">{errors.name}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">NID *</label>
          <input name="nid" value={form.nid} onChange={handleChange} className="mt-1 block w-full border rounded p-2" />
          {errors.nid && <p className="text-red-500 text-sm">{errors.nid}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">Phone *</label>
          <input name="phone" value={form.phone} onChange={handleChange} className="mt-1 block w-full border rounded p-2" />
          {errors.phone && <p className="text-red-500 text-sm">{errors.phone}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">Department *</label>
          <input name="department" value={form.department} onChange={handleChange} className="mt-1 block w-full border rounded p-2" />
          {errors.department && <p className="text-red-500 text-sm">{errors.department}</p>}
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700">Basic Salary *</label>
          <input type="number" name="basicSalary" value={form.basicSalary} onChange={handleChange} className="mt-1 block w-full border rounded p-2" />
          {errors.basicSalary && <p className="text-red-500 text-sm">{errors.basicSalary}</p>}
        </div>

        <div className="border-t pt-4">
          <h2 className="text-lg font-medium mb-2">Spouse (optional)</h2>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700">Name</label>
              <input name="name" value={form.spouse.name} onChange={handleSpouseChange} className="mt-1 block w-full border rounded p-2" />
              {errors.spouseName && <p className="text-red-500 text-sm">{errors.spouseName}</p>}
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700">NID</label>
              <input name="nid" value={form.spouse.nid} onChange={handleSpouseChange} className="mt-1 block w-full border rounded p-2" />
              {errors.spouseNid && <p className="text-red-500 text-sm">{errors.spouseNid}</p>}
            </div>
          </div>
        </div>

        <div className="border-t pt-4">
          <h2 className="text-lg font-medium mb-2">Children</h2>
          {form.children.map((child, idx) => (
            <div key={idx} className="flex gap-4 items-end mb-2">
              <div className="flex-1">
                <label className="block text-sm font-medium text-gray-700">Name</label>
                <input value={child.name} onChange={(e) => handleChildChange(idx, 'name', e.target.value)} className="mt-1 block w-full border rounded p-2" />
                {errors[`childName${idx}`] && <p className="text-red-500 text-sm">{errors[`childName${idx}`]}</p>}
              </div>
              <div className="flex-1">
                <label className="block text-sm font-medium text-gray-700">Date of Birth</label>
                <input type="date" value={child.dateOfBirth} onChange={(e) => handleChildChange(idx, 'dateOfBirth', e.target.value)} className="mt-1 block w-full border rounded p-2" />
                {errors[`childDob${idx}`] && <p className="text-red-500 text-sm">{errors[`childDob${idx}`]}</p>}
              </div>
              <button type="button" onClick={() => removeChild(idx)} className="bg-red-500 text-white px-3 py-1 rounded">Remove</button>
            </div>
          ))}
          <button type="button" onClick={addChild} className="bg-gray-500 text-white px-3 py-1 rounded">Add Child</button>
        </div>

        <div className="flex justify-end space-x-2">
          <button type="button" onClick={() => navigate('/')} className="bg-gray-300 text-gray-700 px-4 py-2 rounded">Cancel</button>
          <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded">Save</button>
        </div>
      </form>
    </div>
  )
}
