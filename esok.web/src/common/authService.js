import { httpClient } from '@/common/httpClient';
import router from '../router';
import Swal from '@/common/sweetalert2';
import passwordValidator from 'password-validator';

class AuthService {
    async signIn(user) {
        user.pending = true;

        await httpClient
            .post('Account/SignIn', {
                email: user.email,
                password: user.password
            })
            .then((result) => {
                localStorage.setItem('auth_token', result.data);

                router
                    .push({ name: "home" })
                    .then(() => { router.go() });
            })
            .catch(() => {
                Swal.toastError('Wystąpił błąd podczas logowania');
            })
            .finally(() => {
                user.pending = false;
            });
    }

    signOut() {
        localStorage.removeItem('auth_token');

        router.push(
            { name: "login" }
        );
    }

    async signUp(user) {
        user.pending = true;

        await httpClient
            .post('Account/SignUp', {
                nameSurname: user.nameSurname,
                email: user.email,
                password: user.password,
                phoneNumber: user.phoneNumber
            })
            .then(() => {
                router.push(
                    { name: "registerSucceed" }
                );
            })
            .catch(() => {
                Swal.toastError('Wystąpił błąd podczas rejestracji');
            })
            .finally(() => {
                user.pending = false;
            });
    }

    async forgotPassword(user) {
        user.pending = true;

        await httpClient
            .post('Account/ForgotPassword/', user.email)
            .then(() => {
                router.push(
                    { name: "forgotPasswordSucceed" }
                );
            })
            .catch(() => {
                Swal.toastError('Wystąpił błąd podczas próby odzyskiwania hasła');
            })
            .finally(() => {
                user.pending = false;
            });
    }

    async confirmEmail(data) {
        data.pending = true;

        await httpClient
            .post('Account/ConfirmEmail/', {
                token: data.token,
                email: data.email
            })
            .then(() => {
                data.confirmSuccess = true
            })
            .catch(() => {
                Swal.toastError('Wystąpił błąd podczas próby aktywowania konta');
                data.confirmSuccess = false
            })
            .finally(() => {
                data.pending = false;
            });
    }

    async resetPassword(data) {
        data.pending = true;

        await httpClient
            .post('Account/ResetPassword/', {
                token: data.token,
                email: data.email,
                password: data.password
            })
            .then(() => {
                Swal.toastSuccess('Hasło zostało zmienione');
                router.push(
                    { name: "login" }
                );
            })
            .catch(() => {
                Swal.toastError('Wystąpił błąd podczas próby zmiany hasła');
            })
            .finally(() => {
                data.pending = false;
            });
    }

    async confirmEmployee(data) {
        data.pending = true;

        await httpClient
            .post('Account/ConfirmEmployee/', {
                token: data.token,
                email: data.email,
                password: data.password
            })
            .then(() => {
                Swal.toastSuccess('Twoje konto zostało aktywowane');
                router.push(
                    { name: "login" }
                );
            })
            .catch(() => {
                Swal.toastError('Wystąpił błąd podczas próby zmiany hasła');
            })
            .finally(() => {
                data.pending = false;
            });
    }

    async changePassword(user) {
        user.pending = true;

        await httpClient
            .post('Account/ChangePassword/', {
                currentPassword: user.currentPassword,
                newPassword: user.newPassword
            })
            .then(() => {
                user.currentPassword = "";
                user.newPassword = "";
                Swal.toastSuccess('Twoje hasło zostało zmienione');
            })
            .catch(() => {
                Swal.toastError('Wystąpił błąd podczas zmiany hasła');
            })
            .finally(() => {
                user.pending = false;
            });
    }

    validatePassword(password) {
        var schema = new passwordValidator();

        schema
            .is().min(6)                                    
            .has().digits()                    
            .has().letters()                                             

        return schema.validate(password);
    }
}

export default new AuthService();