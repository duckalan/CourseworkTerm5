<script setup lang="ts">
import { computed, onBeforeUnmount, onUnmounted, ref, watch } from 'vue';
import { ButtonType, ButtonSize } from '@/enums/ButtonEnums'
import SiteButton from '@/components/SiteButton.vue';
import type { Product } from '@/stores/productCategoriesStore';
import { useShoppingCartStore } from '@/stores/shoppingCartStore';
import PizzaCardModal from './PizzaCardModal.vue';

const shoppingCartStore = useShoppingCartStore();
const isPizzaCardOpen = ref(false);

const props = defineProps<{
    product: Product,
    isPizza: boolean
}>();

watch(isPizzaCardOpen, () => {
    document.body.classList.toggle('js-scroll-lock');
})

const isInCart = ref<boolean>(
    shoppingCartStore.isItemInCart(props.product.productId)
);

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
const unsubscribe = shoppingCartStore.$onAction(
    ({
        name, // name of the action
        store, // store instance, same as `someStore`
        args, // array of parameters passed to the action
        after, // hook after the action returns or resolves
        onError, // hook if the action throws or rejects
    }) => {
        after((result) => {
            if (name === 'removePurchase' && args[0].cartItem.productId === props.product.productId) {
                isInCart.value = false;
            }

            if (name === 'removeItem' && args[0] === props.product.productId) {
                isInCart.value = false;
            }

            if (name === 'removeOneProductUnit' && args[0].productId === props.product.productId) {
                if (result === true) {
                    isInCart.value = false;
                }
            }
        })
    }
)

// manually remove the listener
onBeforeUnmount(() => {
    unsubscribe()
})

function addOneProductUnitToCart() {
    shoppingCartStore.addOneProductUnit({
        productId: props.product.productId,
        name: props.product.name,
        description: props.product.description,
        imageUrl: props.product.imageUrl,
        priceRub: props.product.basePriceRub,
    });
    isInCart.value = true;
}

function removeOneProductUnitFromCart() {
    shoppingCartStore.removeOneProductUnit({
        productId: props.product.productId,
        name: props.product.name,
        description: props.product.description,
        imageUrl: props.product.imageUrl,
        priceRub: props.product.basePriceRub,
    });
}

const isIncrementDisabled = computed(() => {
    return shoppingCartStore.getItemQuantity(props.product.productId) === 9;
})

/*
card
    card__header
        card__image

    card__main
        card__name
        card__description

    card__footer
        card__base-price
            card__price
*/
</script>

<template>
    <article class="card">
        <header class="card__header">
            <img class="card__image"
                 :src="product.imageUrl.toString()"
                 :alt="`Картинка продукта ${product.name}`"
                 loading="lazy">
        </header>

        <main class="card__main">
            <h2 class="card__name">{{ product.name }}</h2>
            <p class="card__description">{{ product.description }}</p>
        </main>

        <footer class="card__footer">

            <template v-if="isPizza">
                <span class="card__base-price">
                    от <span class="card__price">
                        {{ product.basePriceRub }}
                    </span> &#x20bd;
                </span>

                <SiteButton :type="ButtonType.Primary"
                            :size="ButtonSize.Medium"
                            @click="isPizzaCardOpen = true">
                    Выбрать

                    <Teleport to="body">
                        <Transition name="fade">
                            <Suspense>
                                <PizzaCardModal v-if="isPizzaCardOpen"
                                                :product-id="product.productId"
                                                :name="product.name"
                                                :description="product.description ?? ''"
                                                :image-url="product.imageUrl"
                                                @close="isPizzaCardOpen = false" />
                            </Suspense>
                        </Transition>
                    </Teleport>
                </SiteButton>
            </template>

            <template v-else>
                <span class="card__base-price">
                    <span class="card__price">
                        {{ product.basePriceRub }}
                    </span> &#x20bd;
                </span>


                <SiteButton :type="ButtonType.Primary"
                            :size="ButtonSize.Medium"
                            @click="addOneProductUnitToCart"
                            v-if="!isInCart">
                    В корзину
                </SiteButton>


                <template v-else>
                    <div class="quantity-selector">
                        <button class="button-temp button-temp_type_decrement"
                                @click="removeOneProductUnitFromCart()"
                                @click.native.prevent>
                            −
                        </button>
                        <!-- !!!!!!!!!!!!!!!!!!!!!!! -->
                        <input class="quantity-selector__input"
                               type="number"
                               :value="shoppingCartStore.getItemQuantity(product.productId)"

                               readonly>
                        <button class="button-temp button-temp_type_increment"
                                @click="addOneProductUnitToCart()"
                                @click.native.prevent
                                :disabled="isIncrementDisabled">
                            +
                        </button>
                    </div>
                </template>

            </template>

        </footer>
    </article>

</template>

<style scoped lang="less">

/*
.card {
    &__image { ... }
    &__main { ... }
    &__name { ... }
    &__description { ... }
    &__footer { ... }
    &__price { ... }
}
*/
.card {
    color: var(--color-text);
    background: var(--color-background);
    border-radius: var(--border-radius-container);
    padding: 1rem;
    display: flex;
    flex-direction: column;
    justify-content: center;

    &__image {
        width: 100%;
        height: auto;
    }

    &__main {
        flex: 1;
        margin-bottom: 0.75rem;
    }

    &__name {
        font-size: var(--text-lg);
    }

    &__description {
        font-size: var(--text-sm);
        color: var(--color-text-muted);
        line-height: 1.3;
    }

    &__footer {
        display: flex;
        justify-content: space-between;
        align-items: center;
        gap: 1rem;
        flex-wrap: wrap-reverse;
    }

    &__price {
        font-weight: 500;
    }
}

.fade-enter-active,
.fade-leave-active {
    transition: opacity 100ms ease-out;
}

.fade-enter-from,
.fade-leave-to {
    opacity: 0;
}

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
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

.button-temp {
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
