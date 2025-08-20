<template>
    <button v-if="pending" :class=className disabled>
        <span class="spinner-grow spinner-grow-sm"></span>
    </button>

    <button v-else type="submit" :class=className>{{text}}</button>
</template>

<script>
    export default {
        props: {
            pending: Boolean,
            text: String,
            className: String
        }
    }
</script>