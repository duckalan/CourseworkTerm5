<script setup lang="ts">
import { computed, watch } from 'vue';

const model = defineModel<number>({ default: 1 });

const props = defineProps<{
    couldBeZero: boolean,
}>();

const isDecrementDisabled = computed(() => {
    if (props.couldBeZero) {
        return model.value <= 0;
    } else {
        return model.value === 1
    }
})

const isIncrementDisabled = computed(() => {
    return model.value >= 9;
})


function decrement() {
    if (isDecrementDisabled.value) {
        return;
    }

    if (model.value > 1) {
        model.value--;
    }
    else {
        if (model.value === 1 && props.couldBeZero) {
            model.value--;
        }
    }
}

function increment() {
    if (isIncrementDisabled.value) {
        return;
    }

    if (model.value < 9) {
        model.value++;
    }
}

watch(model, () => {
    if (props.couldBeZero) {
        model.value = Math.min(Math.max(0, model.value), 9);
    } else {
        model.value = Math.min(Math.max(1, model.value), 9);
    }
});
</script>

<template>
    <div class="quantity-selector">
        <button class="button button_type_decrement"
                @click="decrement();"
                @click.native.prevent
                
                :disabled="isDecrementDisabled">
            −
        </button>
        <input class="quantity-selector__input"
               type="number"
               v-model.number.lazy="model">
        <button class="button button_type_increment"
                @click="increment()"
                @click.native.prevent
                :disabled="isIncrementDisabled">
            +
        </button>
    </div>
</template>

<style lang="less" scoped>
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button {
    -webkit-appearance: none;
    margin: 0;
}

input[type="number"] {
    -moz-appearance: textfield;
}

.quantity-selector {
    display: flex;
    font-size: 1rem;

    &__input {
        font-size: inherit;
        width: 2rem;
        text-align: center;
        border: none;
        background-color: var(--color-background-soft);
    }
}

.button {
    font-size: inherit;
    padding: 0.5rem;
    transition: background-color 200ms ease-out;
    background-color: var(--brand-color);
    border: none;
    cursor: pointer;
    z-index: 2;

    &:disabled {
        background-color: var(--brand-disabled);
    }

    &_type {
        &_decrement {
            border-top-left-radius: var(--border-radius-item);
            border-bottom-left-radius: var(--border-radius-item);
        }

        &_increment {
            border-top-right-radius: var(--border-radius-item);
            border-bottom-right-radius: var(--border-radius-item);
        }
    }

    &:hover {
        background-color: var(--brand-color-darken);
    }

    &:active {
        outline: 1px solid var(--brand-black);
    }
}
</style>