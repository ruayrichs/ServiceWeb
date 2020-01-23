function showInitiativeModal(modalId) {       
    document.getElementById(modalId).style.width = "100%";
    document.body.style.overflow = 'hidden';
}

function closeInitiativeModal(modalId) {
    document.getElementById(modalId).style.width = "0%";
    document.body.style.overflow = '';
}