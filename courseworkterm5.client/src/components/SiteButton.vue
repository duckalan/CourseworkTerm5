<script setup lang="ts">
import { ButtonType, ButtonSize } from '@/enums/ButtonEnums'
import { computed } from 'vue';

const props = defineProps<{
    type: ButtonType,
    size: ButtonSize,
}>();

const fullClassName = computed(() => {
    return `button button_type_${props.type} button_size_${props.size}`;
});
</script>

<template>
    <button :class="fullClassName">
        <span class="button__text">
            <slot />
        </span>
    </button>
</template>

<style lang="less" scoped>
.button {
    border: none;
    border-radius: var(--border-radius-item);
    padding: 0.5rem 1rem;
    min-width: 5rem;
    cursor: pointer;
    transition: background-color 200ms ease-out;

    &_type {
        &_primary {
            background-color: var(--brand-color);
            color: var(--color-text);

            &:hover {
                background-color: var(--brand-color-darken);
            }

            &:active {
                border: 1px solid var(--brand-black);
            }
        }
    }

    &_size {
        &_small {
            font-size: var(--text-sm);
        }

        &_medium {
            font-size: var(--text-md);
        }

        &_large {
            font-size: var(--text-lg);
        }
    }
}
</style>