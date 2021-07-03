# This is the Denizen script that generated 'minecraft.fds'

meta_extra_data_generator_task:
    type: task
    debug: false
    script:
    - definemap output:
        blocks: <server.material_types.filter[is_block].parse[name].alphabetical>
        items: <server.material_types.filter[is_item].parse[name].alphabetical>
        particles: <server.particle_types.alphabetical>
        effects: <server.effect_types.alphabetical>
        sounds: <server.sound_types.alphabetical>
        entities: <server.entity_types.alphabetical>
        enchantments: <server.enchantment_keys.alphabetical>
        biomes: <server.biome_types.parse[name].alphabetical>
        attributes: <server.nbt_attribute_types.alphabetical>
        gamerules: <server.gamerules.alphabetical>
        potion_effects: <server.potion_effect_types.alphabetical>
        potions: <server.potion_types.alphabetical>
        statistics: <server.statistic_types.alphabetical>
    - log type:none <[output].to_yaml> file:minecraft.fds
