<template>
    <router-view />
</template>

<script>
    export default {
        mounted() {
            var scripts = [
                "/plugins/jquery-ui/jquery-ui.min.js",
                "/plugins/bootstrap/js/bootstrap.bundle.min.js",
                "/plugins/sparklines/sparkline.js",
                "/plugins/jqvmap/jquery.vmap.min.js",
                "/plugins/jqvmap/maps/jquery.vmap.usa.js",
                "/plugins/jquery-knob/jquery.knob.min.js",
                "/plugins/moment/moment.min.js",
                "/plugins/daterangepicker/daterangepicker.js",
                "/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
                "/plugins/summernote/summernote-bs4.min.js",
                "/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
                "/dist/js/adminlte.js"
            ];

            scripts.forEach(script => {
                let tag = document.createElement("script");
                tag.setAttribute("src", script);
                tag.setAttribute('type', 'text/javascript');
                document.body.appendChild(tag);
            });
        }
    }
</script>

<style>
    @import "https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback";
    @import "/public/plugins/fontawesome-free/css/all.min.css";
    @import "/public/plugins/icheck-bootstrap/icheck-bootstrap.min.css";
    @import "/public/dist/css/adminlte.min.css";
    @import "https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback";
    @import "/public/plugins/fontawesome-free/css/all.min.css";
    @import "https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css";
    @import "/public/plugins/icheck-bootstrap/icheck-bootstrap.min.css";
    @import "/public/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css";
    @import "/public/plugins/jqvmap/jqvmap.min.css";
    @import "/public/plugins/overlayScrollbars/css/OverlayScrollbars.min.css";
    @import "/public/plugins/daterangepicker/daterangepicker.css";
    @import "/public/plugins/summernote/summernote-bs4.min.css";
    @import "/public/plugins/bs-stepper/css/bs-stepper.min.css";
    @import "/public/plugins/select2/css/select2.min.css";
</style>
