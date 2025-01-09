<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import type { Product } from '@/stores/productCategoriesStore';
import { useToppingStore } from '@/stores/toppingStore';
import SiteButton from './SiteButton.vue';
import { ButtonSize, ButtonType } from '@/enums/button_enums';
import PizzaVariantRadio from './PizzaVariantRadio.vue';
import type { PizzaVariant } from './PizzaVariantRadio.vue';
import ToppingCardCheckbox, { type ToppingPreview } from './ToppingCardCheckbox.vue';
import QuantitySelector from './QuantitySelector.vue';
import { useShoppingCartStore } from '@/stores/shoppingCartStore';

const shoppingCartStore = useShoppingCartStore();

export type BasePizzaInfo = Omit<Required<Product>, 'basePriceRub'>
const props = defineProps<BasePizzaInfo>();

const toppingStore = useToppingStore();
if (!toppingStore.isLoaded && !toppingStore.isLoading) {
    await toppingStore.fetchToppings();
}

const pizzaVariants = ref<PizzaVariant[]>([]);
const variantsResponse = await fetch(`api/pizzas/${props.productId}`)
const json = await variantsResponse.json();
pizzaVariants.value = json.variants;

const selectedPizzaVariant = ref<PizzaVariant>(pizzaVariants.value[1]);
const toppingPreviews = ref<ToppingPreview[]>(
    toppingStore.toppings
        .map(t => ({
            toppingId: t.toppingId,
            name: t.name,
            imageUrl: t.imageUrl,
            priceRub: t.variants.filter(v => v.diameterCm === selectedPizzaVariant.value.diameterCm)[0].priceRub,
            weightGram: t.variants.filter(v => v.diameterCm === selectedPizzaVariant.value.diameterCm)[0].weightGram,
        }))
);
const selectedToppings = ref<ToppingPreview[]>([]);
const pizzaQuantity = ref(1);

const emit = defineEmits(['close']);

const fullPriceRub = computed(() => {
    return (selectedPizzaVariant.value.priceRub
        + selectedToppings.value.reduce((priceAccum, currentTopping) => {
            return priceAccum + currentTopping.priceRub
        }, 0)) * pizzaQuantity.value
});

// !!!!!!!!!! checked после этого сбрасывается
watch(selectedPizzaVariant, (newVariant, oldVariant) => {
    toppingPreviews.value = toppingStore.toppings
        .map(t => ({
            toppingId: t.toppingId,
            name: t.name,
            imageUrl: t.imageUrl,
            priceRub: t.variants.filter(v => v.diameterCm === newVariant.diameterCm)[0].priceRub,
            weightGram: t.variants.filter(v => v.diameterCm === newVariant.diameterCm)[0].weightGram,
        }));

    //!!!!!!!!!
    selectedToppings.value.length = 0
})

onMounted(() => {
    const modalMask = document.querySelector('.modal-mask');
    modalMask?.addEventListener('click', (event) => {
        if (event.target === event.currentTarget) {
            emit('close');
        }
    })
})

function addToCart() {
    shoppingCartStore.addPizza({
        productId: props.productId,
        name: props.name,
        description: props.description,
        imageUrl: props.imageUrl,
        diameterCm: selectedPizzaVariant.value.diameterCm,
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        priceRub: fullPriceRub.value / pizzaQuantity.value,
        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        toppings: selectedToppings.value,
    }, pizzaQuantity.value);

    emit('close')
}
</script>

<template>
    <div class="modal-mask">
        <section class="pizza-card">
            <header class="pizza-card__header">
                <img class="pizza-card__image" :src="imageUrl.toString()">
            </header>

            <main class="pizza-card__main">
                <div class="pizza-card__title">
                    <h1 class="pizza-card__name">{{ name }}</h1>
                    <p class="pizza-card__description">{{ description }}.</p>
                    <svg class="pizza-card__close-icon" @click="$emit('close')"
                         xmlns="http://www.w3.org/2000/svg" xml:space="preserve" width="14"
                         height="14" version="1.1" viewBox="0 0 14 14"
                         fill="#3a3a3a">
                        <path
                              d="M.3 13.7c.2.2.4.3.7.3.3 0 .5-.1.7-.3L7 8.4l5.3 5.3c.2.2.5.3.7.3.2 0 .5-.1.7-.3.4-.4.4-1 0-1.4L8.4 7l5.3-5.3c.4-.4.4-1 0-1.4-.4-.4-1-.4-1.4 0L7 5.6 1.7.3C1.3-.1.7-.1.3.3c-.4.4-.4 1 0 1.4L5.6 7 .3 12.3c-.4.4-.4 1 0 1.4z" />
                    </svg>
                </div>

                <form class="pizza-card__variant-selection"
                      @submit.prevent="addToCart()">
                    <ul class="pizza-size-selection">
                        <li v-for="variant of pizzaVariants"
                            :class="`pizza-size-selection__size size_${variant.diameterCm}cm`">

                            <PizzaVariantRadio
                                               v-model="selectedPizzaVariant"
                                               :value="variant" />
                        </li>
                    </ul>

                    <div class="topping-selection">
                        <h2 class="topping-selection__title">
                            Добавьте по вкусу
                        </h2>
                        <ul class="topping-selection__toppings">
                            <li class="topping_selection_topping-card"
                                v-for="toppingPreview of toppingPreviews"
                                :key="toppingPreview.name">
                                <ToppingCardCheckbox
                                                     v-model:model-value="selectedToppings"
                                                     :value="toppingPreview" />
                            </li>
                        </ul>
                    </div>

                    <div class="purchase-section">
                        <QuantitySelector v-model="pizzaQuantity"
                                          :could-be-zero="false" />
                        <SiteButton :type="ButtonType.Primary"
                                    :size="ButtonSize.Medium">
                            В корзину за <span class="button__price">{{ fullPriceRub
                                }}</span>&nbsp;&#x20bd;
                        </SiteButton>
                    </div>
                </form>
            </main>
        </section>
    </div>
</template>

<style lang="less" scoped>
.modal-mask {
    min-width: 100vw;
    min-height: 100vh;
    z-index: 9999;
    background-color: rgb(0 0 0 / 0.5);
    position: fixed;
    top: 0;
    left: 0;
    cursor: pointer;
}

.pizza-card {
    --card-padding: 1rem;
    /* !!!!!!!!!!!!!!!!!!!!!!!! */
    width: 900px;
    /* !!!!!!!!!!!!!!!!!!!!!!!! */
    display: flex;
    justify-content: start;
    align-items: center;
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    cursor: auto;

    &__header {
        border-top-left-radius: var(--border-radius-container);
        border-bottom-left-radius: var(--border-radius-container);
        flex: 1 0 50%;
        align-self: stretch;
        background-color: var(--color-background-soft);
        display: flex;
        justify-content: center;
        align-items: center;
        padding: var(--card-padding);
    }

    &__image {
        width: 100%;
        height: auto;
    }

    &__close-icon {
        position: absolute;
        top: calc(var(--card-padding) - 0.5rem);
        right: calc(var(--card-padding) - 0.5rem);
        cursor: pointer;
        width: 2rem;
        height: 2rem;
        padding: 0.5rem;

        &:hover {
            fill: var(--brand-black);
        }
    }

    &__main {
        position: relative;
        border-top-right-radius: var(--border-radius-container);
        border-bottom-right-radius: var(--border-radius-container);

        flex: 1;
        padding: var(--card-padding);
        background-color: var(--color-background);
    }

    &__title {
        margin-bottom: 1.0rem;
    }

    &__name {
        font-size: var(--text-xl);
    }
}

.pizza-size-selection {
    display: flex;
    justify-content: start;
    align-items: center;
    gap: 1.5rem;
    list-style: none;
    margin-bottom: 1.25rem;
}

.topping-selection {
    max-height: 40vh;
    margin-bottom: 1.5rem;
    overflow-y: auto;
    scrollbar-width: thin;

    &__title {
        font-size: var(--text-lg);
    }

    &__toppings {
        list-style: none;
        display: grid;
        gap: 1rem;
        grid-template-columns: repeat(3, 1fr);
        padding: 2px;
    }

    &__topping-card {
        display: inline;
    }
}

.purchase-section {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

// НЕ РАБОТАЕТ
.button {
    &__price {
        font-weight: 500;
    }
}
</style>