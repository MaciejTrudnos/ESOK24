<template>
    <div class="hold-transition sidebar-mini layout-fixed">
        <div class="wrapper">

            <!-- Navbar -->
            <nav class="main-header navbar navbar-expand navbar-white navbar-light">
                <!-- Left navbar links -->
                <ul class="navbar-nav">
                    <li class="nav-item">
                        <a class="nav-link" data-widget="pushmenu" href="#" role="button"><i class="fas fa-bars"></i></a>
                    </li>
                </ul>

                <!-- Right navbar links -->
                <ul class="navbar-nav ml-auto">
                    <li class="nav-item">
                        <a class="nav-link" data-widget="fullscreen" href="#" role="button">
                            <i class="fas fa-expand-arrows-alt"></i>
                        </a>
                    </li>
                    <li class="nav-item dropdown user-menu">
                        <a href="#" class="nav-link dropdown-toggle" data-toggle="dropdown">
                            <span>{{nameSurname}}</span>
                        </a>
                        <ul class="dropdown-menu dropdown-menu-lg dropdown-menu-right">
                            <!-- Menu Footer-->
                            <li class="user-footer">
                                <router-link to="/account" class="btn btn-default btn-flat">
                                    Konto
                                </router-link>

                                <a to="/login" @click="signOut()" class="btn btn-default btn-flat float-right">
                                    Wyloguj
                                </a>
                            </li>
                        </ul>
                    </li>
                </ul>
            </nav>
            <!-- /.navbar -->
            <!-- Main Sidebar Container -->
            <aside class="main-sidebar sidebar-dark-primary elevation-4">
                <!-- Brand Logo -->
                <router-link to="/"  class="brand-link">
                    <img src="dist/img/ESOKLogo.png" alt="ESOK Logo" class="brand-image img-circle elevation-3" style="opacity: .8">
                    <span class="brand-text font-weight-light">ESOK24</span>
                </router-link>

                <!-- Sidebar -->
                <div class="sidebar">

                    <!-- Sidebar Menu -->
                    <nav class="mt-2">
                        <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
                            <!-- Add icons to the links using the .nav-icon class
    with font-awesome or any other icon font library -->
                            <li class="nav-item">
                                <router-link to="/" class="nav-link">
                                    <i class="nav-icon fas fa-th"></i>
                                    <p>
                                        Strona główna
                                    </p>
                                </router-link>
                            </li>
                            <li class="nav-item">
                                <router-link to="/rent" class="nav-link">
                                    <i class="nav-icon fas fa-rocket"></i>
                                    <p>
                                        Wypożyczalnia
                                    </p>
                                </router-link>
                            </li>
                            <li v-if="userRole === 'Manager'" class="nav-item">
                                <router-link to="/reports" class="nav-link">
                                    <i class="nav-icon fas fa-chart-pie"></i>
                                    <p>
                                        Raporty
                                    </p>
                                </router-link>
                            </li>
                            <li class="nav-item">
                                <a href="#" class="nav-link">
                                    <i class="nav-icon fas fa fa-cog"></i>
                                    <p>
                                        Konfiguracja
                                        <i class="fas fa-angle-left right"></i>
                                    </p>
                                </a>
                                <ul class="nav nav-treeview">
                                    <li class="nav-item">
                                        <router-link to="/account" class="nav-link">
                                            <i class="far fa-circle nav-icon"></i>
                                            <p>
                                                Konto
                                            </p>
                                        </router-link>
                                    </li>
                                    <li v-if="userRole === 'Manager'" class="nav-item">
                                        <router-link to="/employees" class="nav-link">
                                            <i class="far fa-circle nav-icon"></i>
                                            <p>
                                                Pracownicy
                                            </p>
                                        </router-link>
                                    </li>
                                    <li v-if="userRole === 'Manager'" class="nav-item">
                                        <router-link to="/products" class="nav-link">
                                            <i class="far fa-circle nav-icon"></i>
                                            <p>
                                                Magazyn produktów
                                            </p>
                                        </router-link>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </nav>
                    <!-- /.sidebar-menu -->
                </div>
                <!-- /.sidebar -->
            </aside>

            <!-- Content Wrapper. Contains page content -->
            <div class="content-wrapper">
                <!-- Content Header (Page header) -->
                <router-view />
                <!-- /.content -->
            </div>
            <!-- /.content-wrapper -->
            <!-- /.content-wrapper -->
            <footer class="main-footer">
                <strong>Prawa autorskie &copy; 2024 <a href="https://esok24.pl">esok24.pl</a>.</strong>
                Wszystkie prawa zastrzeżone.
                <div class="float-right d-none d-sm-inline-block">
                    <b>Wersja</b> 1.1.0
                </div>
            </footer>

            <!-- Control Sidebar -->
            <aside class="control-sidebar control-sidebar-dark">
                <!-- Control sidebar content goes here -->
            </aside>
            <!-- /.control-sidebar -->
        </div>
    </div>
</template>

<script>
    import { decode } from 'jwt-check-expiry';
    import AuthService from '@/common/authService';

    export default {
        data() {
            return {
                nameSurname: '',
                userRole: ''
            }
        },
        mounted: function () {
            const jwtToken = localStorage
                .getItem('auth_token');

            this.nameSurname = jwtToken != null ? decode(jwtToken).payload.nameSurname : '';
            this.userRole = jwtToken != null ? decode(jwtToken).payload.role : '';
        },
        methods: {
            signOut() {
                AuthService.signOut();
            }
        }
    }
</script>