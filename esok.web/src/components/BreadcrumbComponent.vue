<template>
    <div class="content-header">
        <div class="container-fluid">
            <div class="row mb-2">
                <div class="col-sm-6">
                    <h1 class="m-0">{{pageName}}</h1>
                </div><!-- /.col -->
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                        <li v-if="pageName != 'Strona główna'" class="breadcrumb-item">
                            <router-link to="/">
                                Strona główna
                            </router-link>
                        </li>
                        <li class="breadcrumb-item active">{{pageName}}</li>
                    </ol>
                </div><!-- /.col -->
            </div><!-- /.row -->
        </div><!-- /.container-fluid -->
    </div>
</template>

<script>
    export default {
        props: ['pageName']
    }
</script>