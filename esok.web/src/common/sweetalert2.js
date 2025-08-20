import Swal from 'sweetalert2'
import '@sweetalert2/theme-bootstrap-4/bootstrap-4.css'
import "/public/dist/css/sweetalertcustom.css"

const sweetalert2 = {
    toastError(text) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            icon: 'error',
            title: text
        })
    },
    toastSuccess(text) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000,
            timerProgressBar: true,
            icon: 'success',
            title: text
        })
    },
    showSuccessMessage(title, text, showConfirmButton = true, allowOutsideClick = true, allowEscapeKey = true) {
        Swal.fire({
            icon: 'success',
            title: title,
            text: text,
            showConfirmButton: showConfirmButton,
            allowOutsideClick: allowOutsideClick,
            allowEscapeKey: allowEscapeKey
        })
    },
    toastSuccessWithStopTimer(text) {
        Swal.fire({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 60000,
            timerProgressBar: true,
            icon: 'success',
            title: text,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
    },
    async deleteConfirmDialog(title, text) {
        return await Swal.fire({
            title: title,
            text: text,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: 'Nie',
            confirmButtonText: 'Tak, usuń go!'
        }).then((result) => {
            if (result.isConfirmed) {
                return true;
            }

            return false;
        })
    },
    async showLoading() {
        return await Swal.fire({
            title: 'Przetwarzanie...',
            showConfirmButton: false,
            html: '<div class="spinner-grow text-dark"></div>',
            allowOutsideClick: false,
            allowEscapeKey: false
        });
    },
    async deleteRequestDialog(httpRequest, title, text) {
        return await Swal.fire({
            title: title,
            text: text,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: 'Nie',
            confirmButtonText: 'Tak, usuń go!',
            preConfirm: () => {
                sweetalert2.showLoading();
                return httpRequest()
            },
            allowOutsideClick: () => !Swal.isLoading()
        });
    }
}

export default sweetalert2