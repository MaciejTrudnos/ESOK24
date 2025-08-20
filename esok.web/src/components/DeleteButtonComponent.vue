<template>
    <button type="submit" class="btn btn-danger btn-block" >
        <i class="fas fa-trash">
        </i>
        {{text}}
    </button>
</template>

<script>
    export default {
        props: {
            text: String,
        }
    }
</script>