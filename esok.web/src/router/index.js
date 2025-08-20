import { createRouter, createWebHistory } from 'vue-router'
import isJwtTokenExpired from 'jwt-check-expiry';

import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import RegisterView from '../views/RegisterView.vue'
import DashboardView from '../views/DashboardView.vue'
import RentView from '../views/RentView.vue'
import ReportsView from '../views/ReportsView.vue'
import AccountView from '../views/AccountView.vue'
import ProductsView from '../views/ProductsView.vue'
import ForgotPasswordView from '../views/ForgotPasswordView.vue'
import RegisterSucceedView from '../views/RegisterSucceedView.vue'
import ConfirmEmailView from '../views/ConfirmEmailView.vue'
import ForgotPasswordSucceedView from '../views/ForgotPasswordSucceedView.vue'
import ResetPasswordView from '../views/ResetPasswordView.vue'
import EmployeesView from '../views/EmployeesView.vue'
import ConfirmEmployeeView from '../views/ConfirmEmployeeView.vue'

const routes = [
    {
        path: '/login',
        name: 'login',
        component: LoginView
    },
    {
        path: '/register',
        name: 'register',
        component: RegisterView
    },
    {
        path: '/forgotpassword',
        name: 'forgotpassword',
        component: ForgotPasswordView
    },
    {
        path: '/registerSucceed',
        name: 'registerSucceed',
        component: RegisterSucceedView
    },
    {
        path: '/confirmEmail',
        name: 'confirmEmail',
        component: ConfirmEmailView
    },
    {
        path: '/forgotPasswordSucceed',
        name: 'forgotPasswordSucceed',
        component: ForgotPasswordSucceedView
    },
    {
        path: '/resetPassword',
        name: 'resetPassword',
        component: ResetPasswordView
    },
    {
        path: '/confirmEmployee',
        name: 'confirmEmployee',
        component: ConfirmEmployeeView
    },
    {
        path: '/dashboard',
        name: 'dashboard',
        component: DashboardView,
        children: [
            {
                path: '/',
                name: 'home',
                component: HomeView
            },
            {
                path: '/rent',
                name: 'rent',
                component: RentView
            },
            {
                path: '/reports',
                name: 'reports',
                component: ReportsView
            },
            {
                path: '/account',
                name: 'account',
                component: AccountView
            },
            {
                path: '/products',
                name: 'products',
                component: ProductsView
            },
            {
                path: '/employees',
                name: 'employees',
                component: EmployeesView
            }
        ]
    }
]

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes
})

const allowAnonymousPagesName = [
    'register',
    'forgotpassword',
    'registerSucceed',
    'confirmEmail',
    'forgotPasswordSucceed',
    'resetPassword',
    'confirmEmployee'
];

router.beforeEach((to, from, next) => {
    var jwtToken = localStorage
        .getItem('auth_token');

    var isTokenExpired = true;

    if (jwtToken != null) {
        isTokenExpired = isJwtTokenExpired(jwtToken)
    }

    if (allowAnonymousPagesName.includes(to.name)) {
        next();
    } else if (to.name !== 'login' && isTokenExpired) {
        next({ name: 'login' });
    } else {
        next();
    }
})

export default router
