<script setup lang="ts">
import type { Topping } from '@/stores/toppingStore';
import { computed } from 'vue';

export interface ToppingPreview extends Omit<Topping, 'variants'> {
    weightGram: number;
    priceRub: number;
}

const model = defineModel<ToppingPreview[]>()

const props = defineProps<{
    value: ToppingPreview
}>()

const isChecked = computed(() => {
    return model.value?.includes(props.value) ?? false;
})

const toppingCardFullClassname = computed(() => {
    const toppingCardClass = 'topping-card';

    if (isChecked.value) {
        return `${toppingCardClass} ${toppingCardClass}_selected`
    }
    return toppingCardClass;
})
</script>

<template>
    <input type="checkbox"
           v-model="model"
           :value="value"
           :id="value.name"
           name="topping-checkbox"
           hidden>
           
    <label :for="value.name">
        <section :class="toppingCardFullClassname">
            <header class="topping-card__header">
                <img :src="value.imageUrl.toString()"
                     class="topping-card__image">

                <svg class="topping-card__selected-icon" v-if="isChecked"
                     xmlns="http://www.w3.org/2000/svg" width="19.5" height="19.5" fill="none"
                     stroke="#ff970c" viewBox="0 0 19.5 19.5">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-miterlimit="10"
                          stroke-width="1.5" d="M9.75 18.75a9 9 0 1 0 0-18 9 9 0 0 0 0 18z" />
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-miterlimit="10"
                          stroke-width="1.5" d="m5.893 9.75 2.571 2.571 5.143-5.142" />
                </svg>
            </header>

            <main class="topping-card__main">
                <h2 class="topping-card__name">{{ value.name }}</h2>
                <small class="topping-card__weight">{{ value.weightGram }}&nbsp;г</small>
            </main>

            <footer class="topping-card__footer">
                <span class="topping-card__price">{{ value.priceRub }}</span>
                <span class="topping-card__currency">&nbsp;&#x20bd;</span>
            </footer>
        </section>
    </label>
</template>

<style lang="less" scoped>
.topping-card {
    --padding: 0.3rem;
    
    height: 100%;
    color: var(--color-text);
    background-color: var(--color-background-soft);
    border-radius: var(--border-radius-item);
    outline: none;
    padding: var(--padding);
    transition: outline 50ms ease-out;
    text-align: center;
    position: relative;
    cursor: pointer;
    display: flex;
    flex-direction: column;
    justify-content: center;

    &__image {
        width: 100%;
        height: auto;
    }

    &__selected-icon {
        width: 1.15rem;
        height: 1.15rem;
        transition: stroke 50ms ease-out;
        position: absolute;
        top: var(--padding);
        right: var(--padding);
    }

    &__main {
        flex: 1;
        font-size: var(--text-sm);
        margin-bottom: 0.75rem;
        line-height: 1.15;
    }

    &__name {
        font-weight: normal;
        font-size: inherit;
    }

    &__weight {
        color: var(--color-muted-text);
        font-size: inherit;
    }

    &__footer {
        font-size: 1rem;
    }

    &__price {
        font-weight: 500;
    }

    &:hover {
        outline: 2px solid var(--brand-color);
    }

    &:hover &__selected-icon {
        stroke: var(--brand-color-darken);
    }

    &_selected {
        outline: 1px solid var(--brand-color);

        &:hover {
            outline: 2px solid var(--brand-color-darken);
        }
    }
}
</style>