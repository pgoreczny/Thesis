function formatDate(date) {
    const dateParts = date.split('-');
    return dateParts[1] + '-' + dateParts[0] + '-' + dateParts[2];
}