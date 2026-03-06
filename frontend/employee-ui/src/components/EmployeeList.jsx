const downloadPdf = async (id) => {
  try {
    const response = await api.get(`/employees/${id}/pdf`, {
      responseType: 'blob',
    })

    const url = window.URL.createObjectURL(new Blob([response.data]))

    const link = document.createElement('a')
    link.href = url
    link.setAttribute('download', `employee_${id}.pdf`)

    document.body.appendChild(link)
    link.click()

    link.remove()
    window.URL.revokeObjectURL(url)

  } catch (error) {
    alert('PDF download failed')
  }
}
