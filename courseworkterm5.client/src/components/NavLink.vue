<script setup lang="ts">
import { computed } from 'vue'
import { RouterLink, type RouterLinkProps } from 'vue-router'

defineOptions({
    inheritAttrs: false,
});

const props = defineProps<RouterLinkProps>();

const isExternalLink = computed(() => {
    return typeof props.to === 'string' && props.to.startsWith('http')
})
</script>

<template>
    <a v-if="isExternalLink" v-bind="$attrs" :href="to.toString()" target="_blank">
        <slot />
    </a>
    <router-link
                 v-else
                 v-bind="$props"
                 custom
                 v-slot="{ href, navigate }">
        <a
           v-bind="$attrs"
           :href="href"
           @click="navigate"
           class="link">
            <slot />
        </a>
    </router-link>
</template>

<style lang="less" scoped>
.link {
    border: none;
    padding: 0.5rem 0.5rem;
    cursor: pointer;
    font-size: var(--text-md);
    color: var(--color-text);
    transition: color 200ms ease-out, text-decoration-color 200ms ease-out;
    text-decoration: underline 1.5px solid transparent;

    &:hover {
        text-decoration-color: var(--color-text);
    }

    &:active {
        color: var(--brand-color);
        text-decoration-color: var(--brand-color);
    }
}
</style>