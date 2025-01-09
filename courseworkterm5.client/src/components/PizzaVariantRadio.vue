<script setup lang="ts">
import { computed } from 'vue';
import type { Variant } from '@/stores/toppingStore';

export interface PizzaVariant extends Variant {
}

const model = defineModel<PizzaVariant>();

const props = defineProps<{
    value: PizzaVariant
}>();

const isChecked = computed(() => {
    return model.value === props.value
});

const pizzaVariantClass = 'pizza-variant';

const fullPizzaVariantClassname = computed(() => ({
    'pizza-variant': true,
    'pizza-variant_selected': isChecked.value
}))

const fullPizzaVariantCircleClassname = computed(() => {
    const pizzaVariantCircleClass = `${pizzaVariantClass}__circle`;

    return `${pizzaVariantCircleClass} ${pizzaVariantCircleClass}_diameter_${props.value.diameterCm}cm`
})
</script>

<template>
    <input type="radio"
           v-model="model"
           :value="value"
           :id="`pizza-variant-${value.diameterCm}cm`"
           name="pizza-variant"
           hidden
           >

    <label :for="`pizza-variant-${value.diameterCm}cm`">
        <div :class="fullPizzaVariantClassname">
            <div :class="fullPizzaVariantCircleClassname">
                <span class="pizza-variant__diameter">
                    {{ value.diameterCm }}&nbsp;см
                </span>
            </div>
            <div class="pizza-variant__description">
                <div class="pizza-variant__price">
                    <span class="pizza-variant__price-value">{{ value.priceRub }}</span>
                    <span class="pizza-variant__currency">&nbsp;&#x20bd;</span>
                </div>
                <div class="pizza-variant__weight">{{ value.weightGram }}&nbsp;г</div>
            </div>
        </div>
    </label>
</template>

<style lang="less" scoped>
.pizza-variant {
    --25cm-size: 50px;
    --35cm-size: calc(var(--25cm-size) + 10px);
    --42cm-size: calc(var(--35cm-size) + 7px);

    display: flex;
    justify-content: start;
    align-items: center;
    gap: 0.3rem;
    cursor: pointer;

    &__description {
        font-variant: tabular-nums;
    }

    &__price-value {
        font-weight: 500;
    }

    &__weight {
        color: var(--color-muted-text)
    }

    &__circle {
        display: flex;
        justify-content: center;
        align-items: center;
        border-radius: 50%;
        background-color: var(--color-background-soft);
        outline: 0px solid var(--brand-color);
        transition: outline 50ms ease-out, background-color 200ms ease-out;

        &_diameter {
            &_25cm {
                width: var(--25cm-size);
                height: var(--25cm-size);
                outline-width: 1px;
                font-size: var(--text-sm);
            }

            &_35cm {
                width: var(--35cm-size);
                height: var(--35cm-size);
                outline-width: 1.5px;
                font-size: var(--text-md);
            }

            &_42cm {
                width: var(--42cm-size);
                height: var(--42cm-size);
                outline-width: 2px;
                font-size: var(--text-lg);
            }
        }
    }

    &:hover &__circle {
        outline-color: var(--brand-color-darken);

        &_diameter {
            &_25cm {
                outline-width: 2px;
            }

            &_35cm {
                outline-width: 2.5px;
            }

            &_42cm {
                outline-width: 3px;
            }
        }
    }

    &_selected &__circle {
        background-color: var(--brand-color);
        outline: none;
    }
}
</style>